using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces.Repositories;

/// <summary>
/// Represents the refresh token repository.
/// </summary>
public interface IRefreshTokenRepository
{
    /// <summary>Adds a refresh token to the database.</summary>
    /// <param name="refreshToken">The refresh token to add.</param>
    /// <returns>A boolean value indicating whether the operation was successful.</returns>
    Task<bool> AddAsync(RefreshToken refreshToken);

    /// <summary>Gets the refresh token by token.</summary>
    /// <param name="token">The token.</param>
    /// <returns>The refresh token, if found; otherwise, null.</returns>
    Task<RefreshToken?> GetByTokenAsync(string token);

    /// <summary>Marks the refresh token as used.</summary>
    /// <param name="refreshTokenId">The identifier of the refresh token. </param>
    Task MarkAsUsedAsync(Guid refreshTokenId);
}
