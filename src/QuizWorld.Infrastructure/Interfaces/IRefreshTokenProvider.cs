using QuizWorld.Domain.Entities;

namespace QuizWorld.Infrastructure.Interfaces;

/// <summary>
/// Represents the refresh token provider.
/// </summary>
public interface IRefreshTokenProvider
{
    /// <summary>Generates a refresh token.</summary>
    /// <param name="user">The user for which the token is generated.</param>
    /// <returns>The generated refresh token.</returns>
    Task<RefreshToken> Generate(User user);

    /// <summary>Gets the refresh token.</summary>
    /// <param name="token">The token.</param>
    /// <returns>The refresh token.</returns>
    Task<RefreshToken> GetByToken(string token);

    /// <summary>Marks the refresh token as used.</summary>
    /// <param name="refreshTokenId">The identifier of the refresh token.</param>
    Task MarkAsUsed(Guid refreshTokenId);
}
