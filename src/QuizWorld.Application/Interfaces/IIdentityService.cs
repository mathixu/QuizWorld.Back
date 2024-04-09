using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces;

/// <summary>
/// Represents the identity service.
/// </summary>
public interface IIdentityService
{
    /// <summary>Authenticates the user.</summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>The profile and tokens response.</returns>
    Task<ProfileAndTokensResponse> Authenticate(string email, string password);

    /// <summary>
    /// Refreshes the authentication tokens.
    /// </summary>
    /// <param name="refreshToken">The refresh token.</param>
    /// <returns>The tokens response.</returns>
    Task<TokensResponse> RefreshTokens(string token);

    /// <summary>
    /// Generates the authentication tokens.
    /// </summary>
    /// <param name="user">The user for which to generate the tokens.</param>
    /// <returns>The tokens response.</returns>
    Task<TokensResponse> GenerateTokens(User user);
}
