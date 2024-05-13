using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Quizzes.Queries.SearchQuizzes;

public class SearchQuizzesQuery : PaginationQuery, IQuizWorldRequest<PaginatedList<QuizTiny>>
{
    /// <summary>
    /// Represents the name of the quiz.
    /// </summary>
    public string? Name { get; set; } = default!;

    /// <summary>
    /// Indicates whether to search for all quizzes or only the ones created by the current user.
    /// </summary>
    public bool All { get; set; } = false;

    /// <summary>
    /// Represents the status of the quiz. (Pending = 0, Valid = 1, Invalid = 2)
    /// </summary>
    public Status? Status { get; set; } 
}
