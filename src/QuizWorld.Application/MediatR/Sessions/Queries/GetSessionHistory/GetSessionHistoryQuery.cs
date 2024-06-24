using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetSessionHistory;

public class GetSessionHistoryQuery : PaginationQuery, IQuizWorldRequest<PaginatedList<SessionLight>>
{
    /// <summary>
    /// Represents the search query.
    /// </summary>
    public string? Search { get; set; } = default!;
}
