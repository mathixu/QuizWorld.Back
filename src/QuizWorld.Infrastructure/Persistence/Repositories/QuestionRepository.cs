using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Infrastructure.Persistence.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly ILogger<QuestionRepository> _logger;

    private const string QUESTION_COLLECTION = "Question";
    private readonly IMongoCollection<Question> _mongoQuestionCollection;

    public QuestionRepository(IOptions<MongoDbOptions> options, ILogger<QuestionRepository> logger)
    {
        _logger = logger;

        var client = new MongoClient(options.Value.ConnectionString);
        var mongoDb = client.GetDatabase(options.Value.DatabaseName);

        _mongoQuestionCollection = mongoDb.GetCollection<Question>(QUESTION_COLLECTION);
    }

    /// <inheritdoc/>
    public async Task<bool> AddAsync(Question question)
    {
        try
        {
            question.CreatedAt = DateTime.UtcNow;
            question.UpdatedAt = DateTime.UtcNow;

            await _mongoQuestionCollection.InsertOneAsync(question);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add question to the database.");
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<PaginatedList<Question>> GetQuestionsByQuizIdAsync(Guid quizId, int page, int pageSize)
    {
        try
        {
            var filter = Builders<Question>.Filter.Eq(q => q.QuizId, quizId);
            var questions = await _mongoQuestionCollection.Find(filter)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var total = await _mongoQuestionCollection.CountDocumentsAsync(filter);

            return new PaginatedList<Question>(questions, total, page, pageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get questions from the database.");
            return new PaginatedList<Question>(new List<Question>(), 0, page, pageSize);
        }
    }

    /// <inheritdoc/>
    public async Task<Question?> GetByIdAsync(Guid id)
    {
        try
        {
            var filter = Builders<Question>.Filter.Eq(q => q.Id, id);
            var question = await _mongoQuestionCollection.Find(filter).FirstOrDefaultAsync();

            return question;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get question from the database.");
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> AddRangeAsync(IEnumerable<Question> questions)
    {
        try
        {
            foreach (var question in questions)
            {
                question.CreatedAt = DateTime.UtcNow;
                question.UpdatedAt = DateTime.UtcNow;
            }

            await _mongoQuestionCollection.InsertManyAsync(questions);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add questions to the database.");
            return false;
        }
    }

    /// <inheritdoc/>

    public async Task<bool> UpdateQuestionAsync(Guid questionId, Question question)
    {
        try
        {
            var filter = Builders<Question>.Filter.Eq(q => q.Id, questionId);
            question.UpdatedAt = DateTime.UtcNow;

            var result = await _mongoQuestionCollection.ReplaceOneAsync(filter, question);

            return result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update the question in the database.");
            return false;
        }
    }



    /// <inheritdoc/>
    public async Task<bool> UpdateStatus(Guid questionId, Status status)
    {
        try
        {
            var filter = Builders<Question>.Filter.Eq(q => q.Id, questionId);
            var update = Builders<Question>.Update.Set(q => q.Status, status)
                                                                        .Set(q => q.UpdatedAt, DateTime.UtcNow);

            await _mongoQuestionCollection.UpdateOneAsync(filter, update);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update question status in the database.");
            return false;
        }
    }

    public async Task<List<Question>> GetQuestionsByQuizIdAsync(Guid quizId)
    {
        try
        {
            var filter = Builders<Question>.Filter.Eq(q => q.QuizId, quizId);
            return await _mongoQuestionCollection.Find(filter).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get questions from the database.");
            return [];
        }
    }
}
