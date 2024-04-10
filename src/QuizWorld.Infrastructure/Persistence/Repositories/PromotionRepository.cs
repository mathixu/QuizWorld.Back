using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Promotions.Queries.GetPromotions;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Infrastructure.Persistence.Repositories;

/// <summary>
/// The repository for promotions.
/// </summary>
public class PromotionRepository : IPromotionRepository
{
    private readonly ILogger<PromotionRepository> _logger;

    private const string PROMOTION_COLLECTION = "Promotions";
    private readonly IMongoCollection<Promotion> _mongoPromotionCollection;

    public PromotionRepository(IOptions<MongoDbOptions> options, ILogger<PromotionRepository> logger)
    {
        _logger = logger;

        var client = new MongoClient(options.Value.ConnectionString);
        var mongoDb = client.GetDatabase(options.Value.DatabaseName);

        _mongoPromotionCollection = mongoDb.GetCollection<Promotion>(PROMOTION_COLLECTION);
    }

    /// <inheritdoc/>
    public async Task<bool> AddAsync(Promotion promotion)
    {
        try
        {
            promotion.CreatedAt = DateTime.UtcNow;
            promotion.UpdatedAt = DateTime.UtcNow;

            await _mongoPromotionCollection.InsertOneAsync(promotion);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add promotion in the database.");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(Guid promotionId)
    {
        try
        {
            var filter = Builders<Promotion>.Filter.Eq(p => p.Id, promotionId);
            var result = await _mongoPromotionCollection.DeleteOneAsync(filter);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete promotion from the database.");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<Promotion?> GetByIdAsync(Guid promotionId)
    {
        try
        {
            var filter = Builders<Promotion>.Filter.Eq(p => p.Id, promotionId);
            return await _mongoPromotionCollection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get promotion from the database.");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<PaginatedList<Promotion>> GetPromotionsAsync(GetPromotionsQuery query)
    {
        try
        {
            var filters = new List<FilterDefinition<Promotion>>();
            var filterBuilder = Builders<Promotion>.Filter;

            if(!string.IsNullOrWhiteSpace(query.Search))
            {
                var searchFilters = new List<FilterDefinition<Promotion>>
                {
                    filterBuilder.Regex(p => p.NameNormalized, new BsonRegularExpression(query.Search.ToNormalizedFormat(), "i")),
                    filterBuilder.Regex(u => u.IdString, new BsonRegularExpression(query.Search, "i"))
                };

                filters.Add(filterBuilder.Or(searchFilters));
            }

            var finalFilter = filters.Count != 0 ? filterBuilder.And(filters) : filterBuilder.Empty;

            var promotions = await _mongoPromotionCollection.Find(finalFilter)
                .SortBy(u => u.CreatedAt)
                .Skip((query.Page - 1) * query.PageSize)
                .Limit(query.PageSize)
                .ToListAsync();

            var totalPromotions = await _mongoPromotionCollection.CountDocumentsAsync(finalFilter);

            return new PaginatedList<Promotion>(promotions, totalPromotions, query.Page, query.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get promotions from the database.");
            return new PaginatedList<Promotion>(Array.Empty<Promotion>(), 0, query.Page, query.PageSize);
        }

    }
}
