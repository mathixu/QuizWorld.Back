using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;

    private const string USER_COLLECTION = "User";
    private readonly IMongoCollection<User> _mongoUserCollection;

    public UserRepository(IOptions<MongoDbOptions> options, ILogger<UserRepository> logger)
    {
        _logger = logger;

        var client = new MongoClient(options.Value.ConnectionString);
        var mongoDb = client.GetDatabase(options.Value.DatabaseName);

        _mongoUserCollection = mongoDb.GetCollection<User>(USER_COLLECTION);
    }

    /// <inheritdoc/>
    public async Task<bool> AddAsync(User user)
    {
        try
        {
            await _mongoUserCollection.InsertOneAsync(user);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add user to the database.");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<User?> GetByIdAsync(Guid id)
    {
        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            return await _mongoUserCollection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user from the database.");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(Guid userId, User user)
    {
        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update
                .Set(u => u.FullName, user.FullName)
                .Set(u => u.Email, user.Email)
                .Set(u => u.Roles, user.Roles);

            await _mongoUserCollection.UpdateOneAsync(filter, update);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user in the database.");
            return false;
        }
    }
}
