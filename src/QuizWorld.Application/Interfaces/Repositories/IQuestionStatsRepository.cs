using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces.Repositories;

public interface IQuestionStatsRepository
{
    /// <summary>
    /// Adds a question stats entity to the db.
    /// </summary>
    Task<bool> AddAsync(QuestionStats questionStats);

    /// <summary>
    /// Get the question stats by question id.
    /// </summary>
    Task<QuestionStats?> GetByQuestionIdAsync(Guid questionId);

    /// <summary>
    /// Update the question stats entity in the db.
    /// </summary>
    Task<bool> UpdateAsync(Guid questionId, QuestionStats questionStats);
}
