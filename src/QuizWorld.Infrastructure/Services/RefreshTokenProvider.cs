using Microsoft.Extensions.Options;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Common.Options;
using QuizWorld.Infrastructure.Interfaces;

namespace QuizWorld.Infrastructure.Services;

/// <summary>
/// Represents the refresh token provider.
/// </summary>
public class RefreshTokenProvider(IRefreshTokenRepository refreshTokenRepository, IOptions<RefreshTokenOptions> options) : IRefreshTokenProvider
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly RefreshTokenOptions _options = options.Value;

    /// <inheritdoc />
    public async Task<RefreshToken> Generate(User user)
    {
        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            ExpiresAt = DateTime.UtcNow.AddDays(_options.ExpiresInDays),
            User = user.ToTiny()
        };

        await _refreshTokenRepository.AddAsync(refreshToken);

        return refreshToken;
    }

    /// <inheritdoc />
    public async Task<RefreshToken> GetByToken(string token)
    {
        return await _refreshTokenRepository.GetByTokenAsync(token)
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");
    }

    /// <inheritdoc />
    public Task MarkAsUsed(Guid refreshTokenId)
    {
        return _refreshTokenRepository.MarkAsUsedAsync(refreshTokenId);
    }
}
