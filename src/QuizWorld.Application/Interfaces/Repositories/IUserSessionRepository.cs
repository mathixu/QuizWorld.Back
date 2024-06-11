using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces.Repositories;

public interface IUserSessionRepository
{
    /// <summary>Add a new UserSession in database.</summary>
    Task<bool> AddAsync(UserSession userSession);

    /// <summary>Update a UserSession.</summary>
    Task<bool> UpdateAsync(Guid userSessionId, UserSession userSession);

    /// <summary>Get a UserSession by connection id.</summary>
    Task<UserSession?> GetByConnectionIdAsync(string connectionId);

    /// <summary>Get a UserSession by session id.</summary>
    Task<UserSession?> GetBySessionId(Guid sessionId);

    /// <summary>Get a UserSession by id.</summary>
    Task<UserSession?> GetByIdAsync(Guid id);

    /// <summary>
    /// Get the last UserSession by session code and user id.
    /// </summary>
    Task<UserSession?> GetLastBySessionCodeAndUserIdAsync(string code, Guid userId);
}
