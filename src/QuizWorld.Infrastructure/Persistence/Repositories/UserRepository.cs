using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Users.Queries.SearchUsers;
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

    /// <inheritdoc/>
    public async Task<List<User>> GetUsersByIdsAsync(List<Guid> userIds)
    {
        try
        {
            var filter = Builders<User>.Filter.In(u => u.Id, userIds);
            return await _mongoUserCollection.Find(filter).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get users from the database.");
            return [];
        }
    }

    /// <inheritdoc/>
    public async Task<PaginatedList<User>> SearchUsersAsync(SearchUsersQuery query)
    {
        try
        {
            var filter = Builders<User>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                filter = Builders<User>.Filter.Or(
                        Builders<User>.Filter.Regex(u => u.FullName, new BsonRegularExpression(query.Search, "i")),
                        Builders<User>.Filter.Regex(u => u.Email, new BsonRegularExpression(query.Search, "i"))
                    );
            }

            var total = await _mongoUserCollection.CountDocumentsAsync(filter);

            var items = await _mongoUserCollection.Find(filter)
                .SortBy(u => u.FullName)
                .Skip((query.Page - 1) * query.PageSize)
                .Limit(query.PageSize)
                .ToListAsync();

            return new PaginatedList<User>(items, total, query.Page, query.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search for users in the database.");
            return new PaginatedList<User>(new List<User>(), 0, 1, 10);
        }
    }
}
