using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Quizzes.Commands.CreateQuiz;
using QuizWorld.Application.MediatR.Quizzes.Queries.SearchQuizzes;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces;

/// <summary>
/// Represents a service for quiz operations.
/// </summary>
public interface IQuizService
{
    /// <summary>Creates a quiz</summary>
    Task<Quiz> CreateQuizAsync(CreateQuizCommand command);

    /// <summary>Searches quizzes</summary>
    Task<PaginatedList<QuizTiny>> SearchQuizzesAsync(SearchQuizzesQuery query);
}
