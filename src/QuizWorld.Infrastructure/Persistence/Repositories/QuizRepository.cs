using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Quizzes.Queries.SearchQuizzes;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Infrastructure.Persistence.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly ILogger<QuizRepository> _logger;

    private const string QUIZ_COLLECTION = "Quiz";
    private readonly IMongoCollection<Quiz> _mongoQuizCollection;

    public QuizRepository(IOptions<MongoDbOptions> options, ILogger<QuizRepository> logger)
    {
        _logger = logger;

        var client = new MongoClient(options.Value.ConnectionString);
        var mongoDb = client.GetDatabase(options.Value.DatabaseName);

        _mongoQuizCollection = mongoDb.GetCollection<Quiz>(QUIZ_COLLECTION);
    }

    /// <inheritdoc/>
    public async Task<bool> AddAsync(Quiz quiz)
    {
        try
        {
            quiz.CreatedAt = DateTime.UtcNow;
            quiz.UpdatedAt = DateTime.UtcNow;

            await _mongoQuizCollection.InsertOneAsync(quiz);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add quiz to the database.");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<PaginatedList<Quiz>> SearchQuizzesAsync(SearchQuizzesQuery query)
    {
        try
        {
            var filter = Builders<Quiz>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                filter = Builders<Quiz>.Filter.Regex(s => s.NameNormalized, new BsonRegularExpression(query.Name.ToNormalizedFormat(), "i"));
            }

            var total = await _mongoQuizCollection.CountDocumentsAsync(filter);
            var items = await _mongoQuizCollection.Find(filter)
                .SortBy(s => s.Name)
                .Skip((query.Page - 1) * query.PageSize)
                .Limit(query.PageSize)
                .ToListAsync();

            return new PaginatedList<Quiz>(items, total, query.Page, query.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search for quizzes in the database.");
            return new PaginatedList<Quiz>(new List<Quiz>(), 0, 1, 10);
        }
    }
}
