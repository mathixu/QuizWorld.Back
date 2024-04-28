using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Quizzes.Queries.SearchQuizzes;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces.Repositories;

public interface IQuizRepository
{
    /// <summary>Add a new quiz to the database.</summary>
    Task<bool> AddAsync(Quiz quiz);

    /// <summary>Search for quizzes by their name.</summary>
    Task<PaginatedList<Quiz>> SearchQuizzesAsync(SearchQuizzesQuery query);
}
