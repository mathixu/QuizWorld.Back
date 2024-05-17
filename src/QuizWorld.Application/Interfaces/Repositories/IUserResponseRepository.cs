using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces.Repositories;

public interface IUserResponseRepository
{
    /// <summary>
    /// Adds a user response to the repository.
    /// </summary>
    Task<bool> AddAsync(UserResponse userResponse);

    /// <summary>
    /// Gets a user response by session id
    /// </summary>
    Task<UserResponse?> GetUserResponseByUserSessionId(Guid userSessionId);

    /// <summary>
    /// Adds a response to the user response.
    /// </summary>
    Task<bool> AddResponseAsync(Guid userSessionId, Responses response);
}
