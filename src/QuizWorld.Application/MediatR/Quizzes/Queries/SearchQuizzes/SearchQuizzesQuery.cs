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
    /// Represents the status of the quiz. (Unknow = 0, Draft = 1, Pending = 2, Active = 3, Inactive = 4)
    /// </summary>
    public QuizStatus? Status { get; set; } 
}
