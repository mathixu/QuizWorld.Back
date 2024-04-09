using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Infrastructure.Persistence.Repositories;

/// <summary>
/// Represents the refresh token repository.
/// </summary>
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ILogger<RefreshTokenRepository> _logger;

    private const string REFRESH_TOKEN_COLLECTION = "RefreshTokens";
    private readonly IMongoCollection<RefreshToken> _mongoRefreshTokenCollection;

    public RefreshTokenRepository(IOptions<MongoDbOptions> options, ILogger<RefreshTokenRepository> logger)
    {
        _logger = logger;

        var client = new MongoClient(options.Value.ConnectionString);
        var mongoDb = client.GetDatabase(options.Value.DatabaseName);

        _mongoRefreshTokenCollection = mongoDb.GetCollection<RefreshToken>(REFRESH_TOKEN_COLLECTION);
    }

    /// <inheritdoc/>
    public async Task<bool> AddAsync(RefreshToken refreshToken)
    {
        try
        {
            await _mongoRefreshTokenCollection.InsertOneAsync(refreshToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add refresh token to the database.");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        try
        {
            var filter = Builders<RefreshToken>.Filter.Eq(rt => rt.Token, token);

            return await _mongoRefreshTokenCollection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get refresh token by token from the database.");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task MarkAsUsedAsync(Guid refreshTokenId)
    {
        try
        {
            var filter = Builders<RefreshToken>.Filter.Eq(rt => rt.Id, refreshTokenId);
            var update = Builders<RefreshToken>.Update.Set(rt => rt.IsUsed, true);

            await _mongoRefreshTokenCollection.UpdateOneAsync(filter, update);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to mark refresh token as used in the database.");
        }
    }
}
