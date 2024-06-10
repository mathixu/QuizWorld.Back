using AutoMapper;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Questions.Commands.AnswerQuestion;
using QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestion;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Services;

public class QuestionService(IQuestionRepository questionRepository, 
    IQuizService quizService, 
    IQuestionStatsRepository questionStatsRepository, 
    IUserSessionRepository userSessionRepository, 
    IMapper mapper, 
    IQuestionGenerator questionGenerator,
    IUserResponseRepository userResponseRepository
    ) : IQuestionService
{
    private readonly IQuestionRepository _questionRepository = questionRepository;
    private readonly IQuizService _quizService = quizService;
    private readonly IUserSessionRepository _userSessionRepository = userSessionRepository;
    private readonly IQuestionGenerator _questionGenerator = questionGenerator;
    private readonly IMapper _mapper = mapper;
    private readonly IQuestionStatsRepository _questionStatsRepository = questionStatsRepository;
    private readonly IUserResponseRepository _userResponseRepository = userResponseRepository;

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

        var newQuestion = request.Question.ToQuestion(question.QuizId, question.Skill);

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
    
    /// <inheritdoc/>
    public async Task ProcessUserResponse(UserSession userSession, AnswerQuestionCommand command)
    {
        var question = await GetQuestionById(command.QuestionId)
            ?? throw new NotFoundException(nameof(Question), command.QuestionId);

        var questionMinimal = question.ToMinimal();

        if (question.QuizId != command.QuizId)
            throw new BadRequestException("The question does not belong to the quiz.");

        if (!command.AnswerIds.All(x => question.HasAnswer(x)))
            throw new BadRequestException("One or more answers do not belong to the question.");

        var responseIsCorrect = question.CheckAnswer(command.AnswerIds);

        await UpdateUserResponse(userSession.User, command.QuizId, question, responseIsCorrect);

        await UpdateQuestionStats(questionMinimal, responseIsCorrect);
    }

    private async Task UpdateUserResponse(UserTiny user, Guid quizId, Question question, bool isCorrect)
    {
        var userResponse = await _userResponseRepository.GetUserResponse(user.Id, quizId, question.Id);

        if (userResponse is not null)
        {
            userResponse.SuccessRate = (userResponse.SuccessRate * userResponse.Attempts + (isCorrect ? 1 : 0)) / (userResponse.Attempts + 1);
            userResponse.Attempts++;

            await _userResponseRepository.UpdateAsync(userResponse.Id, userResponse);

            return;
        }

        var response = new UserResponse
        {
            User = user,
            QuizId = quizId,
            SkillId = question.Skill.Id,
            Question = question.ToMinimal(),
            Attempts = 1,
            SuccessRate = isCorrect ? 1 : 0
        };

        await _userResponseRepository.AddAsync(response);
    }

    private async Task UpdateQuestionStats(QuestionMinimal question, bool isCorrect)
    {
        var questionStats = await _questionStatsRepository.GetByQuestionIdAsync(question.Id);

        if (questionStats is not null)
        {
            questionStats.SuccessRate = (questionStats.SuccessRate * questionStats.Attempts + (isCorrect ? 1 : 0)) / (questionStats.Attempts + 1);
            questionStats.Attempts++;

            await _questionStatsRepository.UpdateAsync(question.Id, questionStats);

            return;
        } 

        questionStats = new QuestionStats
        {
            Attempts = 1,
            Question = question,
            SuccessRate = isCorrect ? 1 : 0,
        };

        await _questionStatsRepository.AddAsync(questionStats);
    }
}
