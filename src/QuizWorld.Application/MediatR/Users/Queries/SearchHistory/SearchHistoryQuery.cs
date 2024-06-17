using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Users.Queries.SearchHistory;

public class SearchHistoryQuery : PaginationQuery, IQuizWorldRequest<PaginatedList<UserHistory>>
{
    public string? Search { get; set; } = null;
}
