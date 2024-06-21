using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces.Repositories;
public interface IUserAnswerRepository
{
    /// <summary>
    /// Add a UserAnswer.
    /// </summary>
    /// <param name="userAnswer">the userAnswer.</param>
    /// <returns>true if insert, else false.</returns>
    Task<bool> AddAsync(UserAnswer userAnswer);

    /// <summary>
    /// Get the list of UserAnswers of a session.
    /// </summary>
    /// <param name="sessionId">the id of the session.</param>
    /// <param name="userId">the id of the user.</param>
    /// <returns>a list of UserAnswer.</returns>
    Task<List<UserAnswer>> GetUserAnswers(Guid sessionId, Guid userId);
}
