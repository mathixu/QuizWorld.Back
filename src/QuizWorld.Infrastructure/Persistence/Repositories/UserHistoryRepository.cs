using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Sessions.Queries.GetUserHistories;
using QuizWorld.Application.MediatR.Users.Queries.SearchHistory;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Infrastructure.Persistence.Repositories;

public class UserHistoryRepository : IUserHistoryRepository
{
    private readonly ILogger<UserHistoryRepository> _logger;

    private const string USER_HISTORY_COLLECTION = "UserHistory";
    private readonly IMongoCollection<UserHistory> _mongoUserHistoryCollection;

    public UserHistoryRepository(IOptions<MongoDbOptions> options, ILogger<UserHistoryRepository> logger)
    {
        _logger = logger;

        var client = new MongoClient(options.Value.ConnectionString);
        var mongoDb = client.GetDatabase(options.Value.DatabaseName);

        _mongoUserHistoryCollection = mongoDb.GetCollection<UserHistory>(USER_HISTORY_COLLECTION);
    }

    /// <inheritdoc />
    public async Task<bool> AddAsync(UserHistory userHistory)
    {
        try
        {
            await _mongoUserHistoryCollection.InsertOneAsync(userHistory);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add userHistory to the database.");
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> AddUserResultAsync(Guid userId, Guid sessionId, UserSessionResult result)
    {
        try
        {
            var filters = Builders<UserHistory>.Filter.And(
                Builders<UserHistory>.Filter.Eq(uH => uH.User.Id, userId),
                Builders<UserHistory>.Filter.Eq(uH => uH.SessionId, sessionId)
                );

            var update = Builders<UserHistory>.Update.Set(uH => uH.Result, result);

            await _mongoUserHistoryCollection.UpdateOneAsync(filters, update);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add result to this history.");
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<PaginatedList<UserHistory>> SearchHistoriesAsync(Guid userId, SearchHistoryQuery query)
    {
        try
        {
            var filter = Builders<UserHistory>.Filter.Eq(f => f.User.Id, userId);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                filter &= Builders<UserHistory>.Filter.Regex(s => s.Quiz.Name, new BsonRegularExpression(query.Search.ToNormalizedFormat(), "i"));
            }

            var total = await _mongoUserHistoryCollection.CountDocumentsAsync(filter);
            var histories = await _mongoUserHistoryCollection.Find(filter)
                .SortByDescending(s => s.Date)
                .Skip((query.Page - 1) * query.PageSize)
                .Limit(query.PageSize)
                .ToListAsync();

            return new PaginatedList<UserHistory>(histories, total, query.Page, query.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search for user histories in the database.");
            return new PaginatedList<UserHistory>(new List<UserHistory>(), 0, 1, 10);
        }
    }

    /// <inheritdoc />
    public async Task<PaginatedList<UserHistory>> SearchUsersAsync(GetUserHistoriesQuery query)
    {
        try
        {
            var filter = Builders<UserHistory>.Filter.Eq(f => f.SessionId, query.SessionId);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                filter &= Builders<UserHistory>.Filter.Regex(s => s.User.FullName, new BsonRegularExpression(query.Search.ToNormalizedFormat(), "i"));
            }

            var total = await _mongoUserHistoryCollection.CountDocumentsAsync(filter);
            var histories = await _mongoUserHistoryCollection.Find(filter)
                .SortBy(s => s.User.FullName)
                .Skip((query.Page - 1) * query.PageSize)
                .Limit(query.PageSize)
                .ToListAsync();

            return new PaginatedList<UserHistory>(histories, total, query.Page, query.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search for user histories in the database.");
            return new PaginatedList<UserHistory>(new List<UserHistory>(), 0, 1, 10);
        }
    }
}
