using AutoMapper;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestion;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Services;

public class QuestionService(ISkillRepository skillRepository, IQuestionRepository questionRepository, IQuizService quizService, IUserSessionRepository userSessionRepository, IMapper mapper, IQuestionGenerator questionGenerator) : IQuestionService
{
    private readonly IQuestionRepository _questionRepository = questionRepository;
    private readonly IQuizService _quizService = quizService;
    private readonly IUserSessionRepository _userSessionRepository = userSessionRepository;
    private readonly IQuestionGenerator _questionGenerator = questionGenerator;
    private readonly IMapper _mapper = mapper;
    private readonly ISkillRepository _skillRepository = skillRepository;

    /// <inheritdoc/>
    public async Task CreateQuestionsAsync(Quiz quiz)
    {
        List<Question> questions = [];

        foreach (var skillWeight in quiz.SkillWeights)
        {
            var skill = skillWeight.Skill;

            var skillTotalQuestions = GetSkillTotalQuestions(quiz.TotalQuestions, skillWeight.Weight);

            var questionsGenerated = await _questionGenerator.GenerateQuestionsBySkills(quiz.Id, skill, skillTotalQuestions, quiz.Attachment);

            questions.AddRange(questionsGenerated);
        }

        await _questionRepository.AddRangeAsync(questions);
    }
    
    /// <inheritdoc/>
    public async Task<Question> RegenerateQuestion(Guid quizId, Guid questionId, string requirement)
    {
        var question = await _questionRepository.GetByIdAsync(questionId) 
            ?? throw new NotFoundException("Question not found");

        var skill = await _skillRepository.GetById(question.SkillId)
            ?? throw new NotFoundException("Question not found");

        var quiz = await _quizService.GetByIdAsync(quizId) 
            ?? throw new NotFoundException("Quiz not found");

        if (!(question.SkillId == skill.Id && question.QuizId == quiz.Id))
        {
            throw new BadRequestException("The skillId or the quizId doesn't match with the questionId.");
        }

        var regeneratedQuestion = await _questionGenerator.RegenerateQuestion(skill, question, requirement, quiz.Attachment);
        regeneratedQuestion.Id = questionId;

        await _questionRepository.UpdateQuestionAsync(questionId, regeneratedQuestion);

        return regeneratedQuestion;
    }

    /// <inheritdoc/>
    public async Task<List<QuestionTiny>> GetCustomQuestions(Guid quizId, Guid userId)
    {
        var quiz = await _quizService.GetByIdAsync(quizId)
            ?? throw new NotFoundException(nameof(Quiz), quizId);

        var questions = await _questionRepository.GetQuestionsByQuizIdAsync(quizId);

        if (quiz.PersonalizedQuestions)
        {
            // TODO: Add algorithm to get custom questions adapted to the user
        }

        return questions.Take(quiz.TotalQuestions).Select(q => q.ToTiny()).ToList();
    }

    /// <inheritdoc/>
    public async Task<bool> AnswerQuestionAsync(Guid questionId, List<Guid> answerIds)
    {
        var question = await _questionRepository.GetByIdAsync(questionId)
            ?? throw new NotFoundException(nameof(Question), questionId);

        return question.CheckAnswer(answerIds);
    }

    /// <inheritdoc/>
    public async Task<Question?> GetQuestionById(Guid questionId)
    {
        return await _questionRepository.GetByIdAsync(questionId);
    }

    private static int GetSkillTotalQuestions(int quizTotalQuestion, int weight)
    {
        var minimumQuestions = quizTotalQuestion * weight / 100 * 2;

        return minimumQuestions < 1 ? 1 : minimumQuestions;
    }

    /// <inheritdoc/>
    public async Task<Question> UpdateQuestion(UpdateQuestionCommand request)
    {
        var question = await GetQuestionById(request.QuestionId)
            ?? throw new NotFoundException(nameof(Question), request.QuestionId);

        if (question.QuizId != request.QuizId)
        {
            throw new BadRequestException("The quizId of the question does not match with the quizId.");
        }
        var newQuestion = request.Question.ToQuestion(question.QuizId, question.SkillId);
        newQuestion.Id = question.Id;
        newQuestion.CreatedAt = question.CreatedAt;

        await _questionRepository.UpdateQuestionAsync(request.QuestionId, newQuestion);

        return newQuestion;
    }

    /// <inheritdoc/>
    public async Task<Question> UpdateQuestionStatus(Guid quizId, Guid questionId, Status status)
    {
        var question = await GetQuestionById(questionId)
            ?? throw new NotFoundException(nameof(Question), questionId);

        if (question.QuizId != quizId)
        {
            throw new BadRequestException("The quizId of the question does not match with the quizId.");
        }

        question.Status = status;

        await _questionRepository.UpdateStatus(question.Id, status);

        return question;
    }
}
