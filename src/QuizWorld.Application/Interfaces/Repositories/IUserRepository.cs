using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces.Repositories;

/// <summary>
/// The user repository which is used to interact with the user database.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Add a new user to the database.
    /// </summary>
    /// <param name="user">The user to add.</param>
    /// <returns>The result of the operation.</returns>
    Task<bool> AddAsync(User user);

    /// <summary>
    /// Get a user by email.
    /// </summary>
    /// <param name="email">The email of the user to get.</param>
    /// <returns>The user with the specified email or null if not found.</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Get a user by id.
    /// </summary>
    /// <param name="userId">The id of the user to get.</param>
    /// <returns>The user with the specified id or null if not found.</returns>
    Task<User?> GetByIdAsync(Guid userId);
}
