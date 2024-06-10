using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Infrastructure.Persistence.Repositories;

public class QuestionStatsRepository : IQuestionStatsRepository
{
    private readonly ILogger<QuestionStatsRepository> _logger;

    private const string QUESTION_STATS_COLLECTION = "QuestionStats";
    private readonly IMongoCollection<QuestionStats> _mongoQuestionStatsCollection;

    public QuestionStatsRepository(ILogger<QuestionStatsRepository> logger, IOptions<MongoDbOptions> options)
    {
        _logger = logger;

        var client = new MongoClient(options.Value.ConnectionString);
        var mongoDb = client.GetDatabase(options.Value.DatabaseName);

        _mongoQuestionStatsCollection = mongoDb.GetCollection<QuestionStats>(QUESTION_STATS_COLLECTION);
    }

    /// <inheritdoc/>
    public async Task<bool> AddAsync(QuestionStats questionStats)
    {
        try
        {
            await _mongoQuestionStatsCollection.InsertOneAsync(questionStats);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add question stats to the database.");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(Guid questionId, QuestionStats questionStats)
    {
        try
        {
            var filter = Builders<QuestionStats>.Filter.Eq(x => x.Question.Id, questionId);

            await _mongoQuestionStatsCollection.ReplaceOneAsync(filter, questionStats);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update question stats in the database.");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<QuestionStats?> GetByQuestionIdAsync(Guid questionId)
    {
        try
        {
            var filter = Builders<QuestionStats>.Filter.Eq(x => x.Question.Id, questionId);

            return await _mongoQuestionStatsCollection.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get question stats from the database.");
            return null;
        }
    }
}
