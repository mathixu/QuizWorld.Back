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
            var filters = new List<FilterDefinition<User>>();
            var filterBuilder = Builders<User>.Filter;

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var searchFilters = new List<FilterDefinition<User>>
                {
                    filterBuilder.Regex(u => u.EmailNormalized, new BsonRegularExpression(query.Search.ToNormalizedFormat(), "i")),
                    filterBuilder.Regex(u => u.FullNameNormalized, new BsonRegularExpression(query.Search.ToNormalizedFormat(), "i")),
                    filterBuilder.Regex(u => u.IdString, new BsonRegularExpression(query.Search, "i"))
                };

                filters.Add(filterBuilder.Or(searchFilters));
            }

            if (!string.IsNullOrWhiteSpace(query.Promotion))
            {
                var promotionFilters = new List<FilterDefinition<User>>
                {
                    filterBuilder.Regex(u => u.Promotion.Name, new BsonRegularExpression(query.Promotion, "i")),
                    filterBuilder.Regex(u => u.Promotion.IdString, new BsonRegularExpression(query.Promotion, "i"))
                };

                filters.Add(filterBuilder.Or(promotionFilters));
            }

            if (!string.IsNullOrWhiteSpace(query.Role))
            {
                filters.Add(filterBuilder.Eq(u => u.Role, query.Role));
            }

            var finalFilter = filters.Count != 0 ? filterBuilder.And(filters) : filterBuilder.Empty;

            var users = await _mongoUserCollection.Find(finalFilter)
                .SortBy(u => u.CreatedAt)
                .Skip((query.Page - 1) * query.PageSize)
                .Limit(query.PageSize)
                .ToListAsync();

            var totalUsers = await _mongoUserCollection.CountDocumentsAsync(finalFilter);

            return new PaginatedList<User>(users, totalUsers, query.Page, query.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get users from the database.");
            return new PaginatedList<User>(Array.Empty<User>(), 0, query.Page, query.PageSize);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> AddPromotionAsync(Guid userId, PromotionTiny promotion)
    {
        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Set(u => u.Promotion, promotion);

            var result = await _mongoUserCollection.UpdateOneAsync(filter, update);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add promotion to user in the database.");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> RemovePromotionAsync(Guid promotionId)
    {
        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Promotion.Id, promotionId);
            var update = Builders<User>.Update.Set(u => u.Promotion, null);

            await _mongoUserCollection.UpdateManyAsync(filter, update);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove promotion from users in the database.");
            return false;
        }
    }

}
