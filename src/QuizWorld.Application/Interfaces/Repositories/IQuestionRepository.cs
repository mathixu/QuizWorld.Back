using QuizWorld.Application.Common.Models;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Interfaces.Repositories;

public interface IQuestionRepository
{
    /// <summary>Add a new question to the database.</summary>
    Task<bool> AddAsync(Question question);

    /// <summary>Add a range of questions to the database.</summary>
    Task<bool> AddRangeAsync(IEnumerable<Question> questions);
    
    /// <summary>Get a question by its id.</summary>
    Task<Question?> GetByIdAsync(Guid id);

    /// <summary>Get paginated list of questions by quiz id.</summary>
    Task<PaginatedList<Question>> GetQuestionsByQuizIdAsync(Guid quizId, int page, int pageSize);
    /// <summary>Get all questions by quiz id.</summary>
    Task<List<Question>> GetQuestionsByQuizIdAsync(Guid quizId);

    /// <summary>Update the status of a question.</summary>
    /// <param name="questionId">The id of the question to update.</param>
    /// <param name="status">The new status of the question.</param>
    Task<bool> UpdateStatus(Guid questionId, Status status);

    /// <summary>
    /// Update a question/answer.
    /// </summary>
    /// <param name="questionId">the id of the question.</param>
    /// <param name="question">the question to update.</param>
    /// <returns>true if updated, else false.</returns>
    Task<bool> UpdateQuestionAsync(Guid questionId, Question question);

    /// <summary>
    /// Delete all questions from the quiz that are invalid.
    /// </summary>
    /// <param name="quizId"></param>
    Task DeleteInvalidQuestionsByQuizIdAsync(Guid quizId);
}
