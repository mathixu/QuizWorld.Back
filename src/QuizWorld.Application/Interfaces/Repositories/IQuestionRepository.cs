using QuizWorld.Application.Common.Models;
using QuizWorld.Domain.Entities;

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
    Task<PaginatedList<Question>> GetQuestionsByQuizIdAsync(Guid quizId, int page, int pageSize);
}
