using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces.Repositories;

public interface IUserSessionRepository
{
    /// <summary>Add a new UserSession in database.</summary>
    Task<bool> AddAsync(UserSession userSession);

    /// <summary>Update a UserSession.</summary>
    Task<bool> UpdateAsync(UserSession userSession);

    /// <summary>Get a UserSession by connection id.</summary>
    Task<UserSession?> GetByConnectionIdAsync(string connectionId);

    /// <summary>Get a UserSession by session id.</summary>
    Task<UserSession?> GetBySessionId(Guid sessionId);
}
