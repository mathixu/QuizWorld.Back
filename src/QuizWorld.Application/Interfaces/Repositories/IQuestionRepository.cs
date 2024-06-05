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

    /// <summary>Get all questions by quiz id.</summary>
    Task<List<Question>> GetQuestionsByQuizIdAsync(Guid quizId);

    /// <summary>Update the status of a question.</summary>
    /// <param name="questionId">The id of the question to update.</param>
    /// <param name="status">The new status of the question.</param>
    Task<bool> UpdateStatus(Guid questionId, Status status);
}
