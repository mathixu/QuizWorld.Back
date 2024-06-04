using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizWorld.Infrastructure.Common.Models;
using QuizWorld.Infrastructure.Common.Options;
using QuizWorld.Infrastructure.Interfaces;

namespace QuizWorld.Infrastructure.Persistence.Repositories;

public class GeneratedContentRepository : IGeneratedContentRepository
{
    private readonly ILogger<GeneratedContentRepository> _logger;

    private const string GENERATED_CONTENT_COLLECTION = "GeneratedContent";
    private readonly IMongoCollection<GeneratedContent> _mongoGeneratedContentCollection;

    public GeneratedContentRepository(IOptions<MongoDbOptions> options, ILogger<GeneratedContentRepository> logger)
    {
        _logger = logger;

        var client = new MongoClient(options.Value.ConnectionString);
        var mongoDb = client.GetDatabase(options.Value.DatabaseName);

        _mongoGeneratedContentCollection = mongoDb.GetCollection<GeneratedContent>(GENERATED_CONTENT_COLLECTION);
    }

    /// <inheritdoc/>
    public async Task<bool> AddAsync(GeneratedContent content)
    {
        try
        {
            content.CreatedAt = DateTime.UtcNow;
            content.UpdatedAt = DateTime.UtcNow;

            await _mongoGeneratedContentCollection.InsertOneAsync(content);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add generated content to the database.");
            return false;
        }
    }
}
