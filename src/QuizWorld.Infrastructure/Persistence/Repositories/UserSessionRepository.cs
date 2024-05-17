using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Infrastructure.Persistence.Repositories;

public class UserSessionRepository : IUserSessionRepository
{
    private readonly ILogger<UserSessionRepository> _logger;

    private const string USERSESSION_COLLECTION = "UserSession";
    private readonly IMongoCollection<UserSession> _mongoUserSessionCollection;

    public UserSessionRepository(IOptions<MongoDbOptions> options, ILogger<UserSessionRepository> logger)
    {
        _logger = logger;

        var client = new MongoClient(options.Value.ConnectionString);
        var mongoDb = client.GetDatabase(options.Value.DatabaseName);

        _mongoUserSessionCollection = mongoDb.GetCollection<UserSession>(USERSESSION_COLLECTION);
    }

    /// <inheritdoc/>
    public async Task<bool> AddAsync(UserSession userSession)
    {
        try
        {
            await _mongoUserSessionCollection.InsertOneAsync(userSession);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add user session to the database.");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(UserSession userSession)
    {
        try
        {
            var filter = Builders<UserSession>.Filter.Eq(s => s.Id, userSession.Id);
            var update = Builders<UserSession>.Update
                .Set(s => s.ConnectionId, userSession.ConnectionId)
                .Set(s => s.User, userSession.User)
                .Set(s => s.Session, userSession.Session)
                .Set(s => s.Status, userSession.Status)
                .Set(s => s.DisconnectedAt, userSession.DisconnectedAt);

            await _mongoUserSessionCollection.UpdateOneAsync(filter, update);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user session in the database.");
            return false;
        }
    }

    public async Task<UserSession?> GetByConnectionIdAsync(string connectionId)
    {
        try
        {
            var userSession = await _mongoUserSessionCollection.Find(s => s.ConnectionId == connectionId).FirstOrDefaultAsync();
            return userSession;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user session by connection id from the database.");
            return null;
        }
    }

    public async Task<UserSession?> GetBySessionId(Guid sessionId)
    {
        try
        {
            var userSession = await _mongoUserSessionCollection.Find(s => s.Session.Id == sessionId).FirstOrDefaultAsync();
            return userSession;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user session by session id from the database.");
            return null;
        }
    }
}
