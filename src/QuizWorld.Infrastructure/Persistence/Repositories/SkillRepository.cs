using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Skills.Queries.SearchSkills;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Infrastructure.Persistence.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly ILogger<SkillRepository> _logger;

    private const string SKILL_COLLECTION = "Skill";
    private readonly IMongoCollection<Skill> _mongoSkillCollection;

    public SkillRepository(IOptions<MongoDbOptions> options, ILogger<SkillRepository> logger)
    {
        _logger = logger;

        var client = new MongoClient(options.Value.ConnectionString);
        var mongoDb = client.GetDatabase(options.Value.DatabaseName);

        _mongoSkillCollection = mongoDb.GetCollection<Skill>(SKILL_COLLECTION);
    }

    /// <inheritdoc/>
    public async Task<bool> AddAsync(Skill skill)
    {
        try
        {
            await _mongoSkillCollection.InsertOneAsync(skill);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add skill to the database.");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<List<Skill>> GetSkillsByIdsAsync(IEnumerable<Guid> skillIds)
    {
        try
        {
            var filter = Builders<Skill>.Filter.In(s => s.Id, skillIds);

            return await _mongoSkillCollection.Find(filter).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get skills by ids from the database.");
            return [];
        }
    }

    // TODO: Add name as Index
    /// <inheritdoc/>
    public async Task<Skill?> GetSkillByName(string name)
    {
        try
        {
            var filter = Builders<Skill>.Filter.Eq(s => s.NameNormalized, name.ToNormalizedFormat());

            return await _mongoSkillCollection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get skill by name from the database.");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<PaginatedList<Skill>> SearchSkillsAsync(SearchSkillsQuery query)
    {
        try
        {
            var filter = Builders<Skill>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                filter = Builders<Skill>.Filter.Regex(s => s.NameNormalized, new BsonRegularExpression(query.Name.ToNormalizedFormat(), "i"));
            }

            var totalSkills = await _mongoSkillCollection.CountDocumentsAsync(filter);
            var skills = await _mongoSkillCollection.Find(filter)
                .SortBy(s => s.Name)
                .Skip((query.Page - 1) * query.PageSize)
                .Limit(query.PageSize)
                .ToListAsync();

            return new PaginatedList<Skill>(skills, totalSkills, query.Page, query.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search for skills in the database.");
            return new PaginatedList<Skill>(new List<Skill>(), 0, 1, 10);
        }
    }
    
    /// <inheritdoc/>
    public async Task<Skill> GetById(Guid id)
    {
        try
        {
            var filter = Builders<Skill>.Filter.Eq(s => s.Id, id);

            return await _mongoSkillCollection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get skill by id from the database.");
            return null;
        }
    }
}
