using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Infrastructure.Persistence.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly ILogger<SessionRepository> _logger;

    private const string SESSION_COLLECTION = "Session";
    private readonly IMongoCollection<Session> _mongoSessionCollection;

    public SessionRepository(IOptions<MongoDbOptions> options, ILogger<SessionRepository> logger)
    {
        _logger = logger;

        var client = new MongoClient(options.Value.ConnectionString);
        var mongoDb = client.GetDatabase(options.Value.DatabaseName);

        _mongoSessionCollection = mongoDb.GetCollection<Session>(SESSION_COLLECTION);
    }

    /// <inheritdoc/>
    public async Task<bool> AddAsync(Session session)
    {
        try
        {
            session.CreatedAt = DateTime.UtcNow;
            session.UpdatedAt = DateTime.UtcNow;

            await _mongoSessionCollection.InsertOneAsync(session);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add session to the database.");
            return false;
        }
    }

    // TODO: Add code as Index
    /// <inheritdoc/>
    public async Task<Session?> GetByCodeAsync(string code)
    {
        try
        {
            var session = await _mongoSessionCollection.Find(s => s.Code == code).FirstOrDefaultAsync();
            return session;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get session by code from the database.");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<Session?> GetByIdAsync(Guid sessionId)
    {
        try
        {
            var session = await _mongoSessionCollection.Find(s => s.Id == sessionId).FirstOrDefaultAsync();
            return session;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get session by id from the database.");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateStatusAsync(Guid sessionId, SessionStatus status)
    {
        try
        {
            var filter = Builders<Session>.Filter.Eq(s => s.Id, sessionId);
            var update = Builders<Session>.Update.Set(s => s.Status, status)
                .Set(s => s.UpdatedAt, DateTime.UtcNow);

            await _mongoSessionCollection.UpdateOneAsync(filter, update);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update session status in the database.");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateSessionAsync(Guid sessionId, Session session)
    {
        try
        {
            var filter = Builders<Session>.Filter.Eq(s => s.Id, sessionId);

            await _mongoSessionCollection.ReplaceOneAsync(filter, session);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update session in the database.");
            return false;
        }
    }
}
