using QuizWorld.Application.MediatR.Quizzes.Commands.StartQuiz;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Interfaces;

public interface ISessionService
{
    /// <summary>Creates a session with the given quiz ids.</summary>
    /// <param name="quizId">Represents the id of the quizz.</param>
    /// <returns>Returns the created session.</returns>
    Task<Session> CreateSession(Guid quizId);

    /// <summary>Gets the session.</summary>
    /// <param name="code">The code of the session.</param>
    /// <returns>Returns the session.</returns>
    Task<Session?> GetSessionByCode(string code);

    /// <summary>Adds a user to a session.</summary>
    /// <param name="code">The code of the session.</param>
    /// <param name="connectionId">The connection id of the user.</param>
    /// <param name="user">The user to add to the session.</param>
    Task<UserSession> AddUserSession(string code, string connectionId, User user);

    /// <summary>Changes the status of a user session.</summary>
    /// <param name="connectionId">The connection id of the user session.</param>
    /// <param name="status">The new status of the user session.</param>
    Task ChangeUserSessionStatus(string connectionId, UserSessionStatus status);

    /// <summary>User starts a quiz.</summary>
    Task<StartQuizResponse> StartQuiz(Guid quizId);

    /// <summary>Gets the current user session.</summary>
    Task<UserSession> GetCurrentUserSession();

    /// <summary>
    /// Updates the status of a session.
    /// </summary>
    Task<Session> UpdateSessionStatus(string code, SessionStatus status);

    /// <summary>
    /// Gets the session result.
    /// </summary>
    Task<UserSessionResult?> GetSessionResult(string code);
}
