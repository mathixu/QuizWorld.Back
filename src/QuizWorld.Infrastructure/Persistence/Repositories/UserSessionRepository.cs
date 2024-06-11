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
    public async Task<bool> UpdateAsync(Guid userSessionId, UserSession userSession)
    {
        try
        {
            var filter = Builders<UserSession>.Filter.Eq(s => s.Id, userSessionId);

            await _mongoUserSessionCollection.ReplaceOneAsync(filter, userSession);

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

    /// <inheritdoc/>
    public async Task<UserSession?> GetByIdAsync(Guid id)
    {
        try
        {
            var userSession = await _mongoUserSessionCollection.Find(s => s.Id == id).FirstOrDefaultAsync();

            return userSession;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user session by id from the database.");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<UserSession?> GetLastBySessionCodeAndUserIdAsync(string code, Guid userId)
    {
        try
        {
            var userSession = await _mongoUserSessionCollection
                .Find(s => s.Session.Code == code && s.User.Id == userId)
                .SortByDescending(s => s.ConnectedAt)
                .FirstOrDefaultAsync();

            return userSession;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get last user session by session code and user id from the database.");
            return null;
        }
    }
}
