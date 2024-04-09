using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Users.Queries.GetUsers;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Infrastructure.Persistence.Repositories;

/// <summary>
/// Represents the user repository which is used to interact with the user database.
/// </summary>
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
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

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
    public async Task<User?> GetByEmailAsync(string email)
    {
        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.EmailNormalized, email.ToNormalizedFormat());

            return await _mongoUserCollection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user by email from the database.");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<User?> GetByIdAsync(Guid userId)
    {
        try
        {
            return await _mongoUserCollection.Find(u => u.Id == userId).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user by id from the database.");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateRole(Guid userId, string role)
    {
        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Set(u => u.Role, role);

            await _mongoUserCollection.UpdateOneAsync(filter, update);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user role in the database.");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<PaginatedList<User>> GetUsersAsync(GetUsersQuery query)
    {
        try
        {
            var filter = Builders<User>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var searchFilter = Builders<User>.Filter.Or(
                    Builders<User>.Filter.Regex(u => u.EmailNormalized, new BsonRegularExpression(query.Search, "i")),
                    Builders<User>.Filter.Regex(u => u.FullNameNormalized, new BsonRegularExpression(query.Search, "i")),
                    Builders<User>.Filter.Regex(u => u.IdString, new BsonRegularExpression(query.Search, "i"))
                );

                filter = Builders<User>.Filter.And(filter, searchFilter);
            }

            if (!string.IsNullOrWhiteSpace(query.Promotion))
            {
                var promotionFilter = Builders<User>.Filter.Or(
                    Builders<User>.Filter.Regex(u => u.Promotion.Name, new BsonRegularExpression(query.Promotion, "i")),
                    Builders<User>.Filter.Regex(u => u.Promotion.IdString, new BsonRegularExpression(query.Promotion, "i"))
                );

                filter = Builders<User>.Filter.And(filter, promotionFilter);
            }

            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                filter = Builders<User>.Filter.And(filter, Builders<User>.Filter.Eq(u => u.Role, query.Role));
            }

            var users = await _mongoUserCollection.Find(filter)
                .SortBy(u => u.CreatedAt)
                .Skip((query.Page - 1) * query.PageSize)
                .Limit(query.PageSize)
                .ToListAsync();

            var totalUsers = await _mongoUserCollection.CountDocumentsAsync(filter);

            return new PaginatedList<User>(users, (int)totalUsers, query.Page, query.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get users from the database.");
            return new PaginatedList<User>(new List<User>(), 0, query.Page, query.PageSize);
        }
    }
}
