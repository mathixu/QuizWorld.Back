using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Quizzes.Queries.SearchQuizzes;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Interfaces.Repositories;

public interface IQuizRepository
{
    /// <summary>Add a new quiz to the database.</summary>
    Task<bool> AddAsync(Quiz quiz);

    /// <summary>Search for quizzes by their name.</summary>
    Task<PaginatedList<Quiz>> SearchQuizzesAsync(SearchQuizzesQuery query);

    /// <summary>Get a quiz by its id.</summary>
    Task<Quiz?> GetByIdAsync(Guid id);

    /// <summary>Add an attachment to a quiz.</summary>
    Task<bool> UpdateAttachmentToQuizAsync(Guid quizId, QuizFile file);

    /// <summary>Gets quizzes by their ids</summary>
    Task<List<Quiz>> GetQuizzesByIds(List<Guid> ids);

    /// <summary>Update the status of a quiz.</summary>
    Task<bool> UpdateStatusAsync(Guid quizId, QuizStatus status);
}
