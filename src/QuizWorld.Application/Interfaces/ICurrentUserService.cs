namespace QuizWorld.Application.Interfaces;

/// <summary>
/// Used to get the current user.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the email of the current user.
    /// </summary>
    string? UserEmail { get; }

    /// <summary>
    /// Gets the id of the current user.
    /// </summary>
    Guid? UserId { get; }

    /// <summary>
    /// Gets the token of the current user.
    /// </summary>
    string? UserToken { get; }
}
