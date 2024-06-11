using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces.Repositories;

public interface IUserResponseRepository
{
    /// <summary>
    /// Adds a user response to the db.
    /// </summary>
    Task<bool> AddAsync(UserResponse userResponse);

    /// <summary>
    /// Gets the user response by the user id, quiz id, and question id.
    /// </summary>
    Task<UserResponse?> GetUserResponse(Guid userId, Guid quizId, Guid questionId);

    /// <summary>
    /// Updates the user response in the db.
    /// </summary>
    Task<bool> UpdateAsync(Guid userResponseId, UserResponse userResponse);
}
