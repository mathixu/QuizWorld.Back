using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces;

/// <summary>
/// Represents the service for the questions.
/// </summary>
public interface IQuestionService
{
    /// <summary>
    /// Creates the questions for the quiz.
    /// </summary>
    Task CreateQuestionsAsync(Quiz quiz);

    /// <summary>
    /// Gets the custom questions for a User.
    /// </summary>
    Task<List<QuestionTiny>> GetCustomQuestions(Guid quizId, Guid userId);

    /// <summary>
    /// Answers a question.
    /// </summary>
    Task<bool> AnswerQuestionAsync(Guid questionId, List<Guid> answerIds);

    /// <summary>
    /// Gets a question by its id.
    /// </summary>
    Task<Question?> GetQuestionById(Guid questionId);

    /// <summary>
    /// Edit a question.
    /// </summary>
    Task<QuestionTiny> UpdateQuestion(Guid quizId, Guid questionId, string questionName, List<Guid>? answerIds = null);
}
