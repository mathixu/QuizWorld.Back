using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Services;

public class QuestionService(IQuestionRepository questionRepository, IQuizService quizService, IUserSessionRepository userSessionRepository) : IQuestionService
{
    private readonly IQuestionRepository _questionRepository = questionRepository;
    private readonly IQuizService _quizService = quizService;
    private readonly IUserSessionRepository _userSessionRepository = userSessionRepository;

    // Fake implementation
    /// <inheritdoc/>
    public async Task CreateQuestionsAsync(Quiz quiz)
    {
        var questions = new List<Question>();

        for(int i = 0; i < quiz.TotalQuestions; i++)
        {
            var question = new Question
            {
                Text = $"How much is {i} + {i * 2}?",
                Type = QuestionType.SimpleChoice,
                Answers = [
                    new Answer { Text = $"{i + i * 2}", IsCorrect = true },
                    new Answer { Text = $"{i + i * 2 + 1}", IsCorrect = false },
                    new Answer { Text = $"{i + i * 2 + 2}", IsCorrect = false },
                    new Answer { Text = $"{i + i * 2 + 3}", IsCorrect = false },
                    new Answer { Text = $"{i + i * 2 + 4}", IsCorrect = false },
                ],
                QuizId = quiz.Id,
            };

            for(int j = 0; j < i%3; j++)
            {
                question.Answers.Add(new Answer { Text = $"{i + i * 2 + j + 5}", IsCorrect = false });
            }

            questions.Add(question);
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
    public async Task<bool> AnswerQuestionAsync(Guid questionId, List<Guid> AnswerIds)
    {
        var question = await _questionRepository.GetByIdAsync(questionId)
            ?? throw new NotFoundException(nameof(Question), questionId);

        return question.CheckAnswer(AnswerIds);
    }

    /// <inheritdoc/>
    public async Task<Question?> GetQuestionById(Guid questionId)
    {
        return await _questionRepository.GetByIdAsync(questionId);
    }
}
