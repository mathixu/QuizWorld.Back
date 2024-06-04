using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Services;

public class QuestionService(IQuestionRepository questionRepository, IQuizService quizService, IUserSessionRepository userSessionRepository, IQuestionGenerator questionGenerator) : IQuestionService
{
    private readonly IQuestionRepository _questionRepository = questionRepository;
    private readonly IQuizService _quizService = quizService;
    private readonly IUserSessionRepository _userSessionRepository = userSessionRepository;
    private readonly IQuestionGenerator _questionGenerator = questionGenerator;

    /// <inheritdoc/>
    public async Task CreateQuestionsAsync(Quiz quiz)
    {
        List<Question> questions = [];

        foreach(var skillWeight in quiz.SkillWeights)
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

    /// <inheritdoc/>
    public async Task<QuestionTiny> UpdateQuestion(Guid quizId, Guid questionId, QuestionTiny newQuestion)
    {
        var question = await GetQuestionById(questionId) 
            ?? throw new NotFoundException(nameof(QuestionTiny), questionId);

        var questionTiny = new QuestionTiny();

        if (question.QuizId == quizId)
        {
            if (newQuestion != null)
            {
                questionTiny = await _questionRepository.UpdateQuestion(question.ToTiny(), newQuestion);
            }
        }

        return questionTiny;
    }
}
