using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Infrastructure.Persistence.Repositories;

public class UserResponseRepository : IUserResponseRepository
{
    private readonly ILogger<UserResponseRepository> _logger;

    private const string USER_RESPONSE_COLLECTION = "UserResponse";
    private readonly IMongoCollection<UserResponse> _mongoUserResponseCollection;

    public UserResponseRepository(IOptions<MongoDbOptions> options, ILogger<UserResponseRepository> logger)
    {
        _logger = logger;

        var client = new MongoClient(options.Value.ConnectionString);
        var mongoDb = client.GetDatabase(options.Value.DatabaseName);

        _mongoUserResponseCollection = mongoDb.GetCollection<UserResponse>(USER_RESPONSE_COLLECTION);
    }

    /// <inheritdoc/>
    public async Task<bool> AddAsync(UserResponse userResponse)
    {
        try
        {
            userResponse.CreatedAt = DateTime.UtcNow;
            userResponse.UpdatedAt = DateTime.UtcNow;

            await _mongoUserResponseCollection.InsertOneAsync(userResponse);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add user response to the database.");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<UserResponse?> GetUserResponseByUserSessionId(Guid userSessionId)
    {
        try
        {
            var userResponse = await _mongoUserResponseCollection.Find(ur => ur.UserSessionId == userSessionId).FirstOrDefaultAsync();
            return userResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user response by user session id from the database.");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> AddResponseAsync(Guid userSessionId, Responses response)
    {
        try
        {
            var filter = Builders<UserResponse>.Filter.Eq(ur => ur.UserSessionId, userSessionId);
            var update = Builders<UserResponse>.Update.Push(ur => ur.Responses, response);

            await _mongoUserResponseCollection.UpdateOneAsync(filter, update);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add response to user response in the database.");
            return false;
        }
    }
}
