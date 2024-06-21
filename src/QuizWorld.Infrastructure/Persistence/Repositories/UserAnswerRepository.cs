using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Infrastructure.Persistence.Repositories;
public class UserAnswerRepository : IUserAnswerRepository
{
    private readonly ILogger<UserAnswerRepository> _logger;

    private const string USER_ANSWER_COLLECTION = "UserAnswer";
    private readonly IMongoCollection<UserAnswer> _mongoUserAnswerCollection;

    public UserAnswerRepository(IOptions<MongoDbOptions> options, ILogger<UserAnswerRepository> logger)
    {
        _logger = logger;

        var client = new MongoClient(options.Value.ConnectionString);
        var mongoDb = client.GetDatabase(options.Value.DatabaseName);

        _mongoUserAnswerCollection = mongoDb.GetCollection<UserAnswer>(USER_ANSWER_COLLECTION);
    }

    /// <inheritdoc />
    public async Task<bool> AddAsync(UserAnswer userAnswer)
    {
        try
        {
            await _mongoUserAnswerCollection.InsertOneAsync(userAnswer);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add user answer to the database.");
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<List<UserAnswer>> GetUserAnswers(Guid sessionId, Guid userId)
    {
        var filter = Builders<UserAnswer>.Filter.And(
            Builders<UserAnswer>.Filter.Eq(ua => ua.SessionId, sessionId),
            Builders<UserAnswer>.Filter.Eq(ua => ua.UserId, userId)
        );
        return await _mongoUserAnswerCollection.Find(filter).ToListAsync();
    }
}
