using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Users.Queries.SearchUsers;

public class SearchUsersQuery : PaginationQuery, IQuizWorldRequest<PaginatedList<UserTiny>>
{
    /// <summary>
    /// Represents the search query.
    /// </summary>
    public string? Search { get; set; } = default!;
}
