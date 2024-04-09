using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.MediatR.Common;

namespace QuizWorld.Application.MediatR.Users.Queries.GetUsers;

/// <summary>
/// Represents the query for getting users.
/// </summary>
public class GetUsersQuery : PaginationQuery, IQuizWorldRequest<PaginatedList<ProfileResponse>>
{
    /// <summary>
    /// Search by email, full name or id.
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// Filter by promotion name or id.
    /// </summary>
    public string? Promotion { get; set; }

    /// <summary>
    /// Filter by role. 
    /// </summary>
    public string? Role { get; set; }
}
