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
}
