using QuizWorld.Domain.Entities;
using System.Security.Claims;

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
    /// Gets the full name of the current user.
    /// </summary>
    string? UserFullName { get; }

    /// <summary>
    /// Gets the token of the current user.
    /// </summary>
    string? UserToken { get; }

    /// <summary>
    /// Gets the roles of the current user.
    /// </summary>
    string[]? UserRoles { get; }

    /// <summary>
    /// Gets the user information.
    /// </summary>
    User? User { get; }

    /// <summary>
    /// Gets the user information with minimal information.
    /// </summary>
    UserTiny? UserTiny { get; }

    /// <summary>Extracts the user from the claims principal.</summary>
    /// <param name="claimsPrincipal">The claims principal.</param>
    /// <returns>The user.</returns>
    User? ExtractUser(ClaimsPrincipal claimsPrincipal);
}
