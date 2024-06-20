using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;
using System.Text.Json.Serialization;

namespace QuizWorld.Application.MediatR.Users.Queries.SearchHistory;

public class SearchHistoryQuery : PaginationQuery, IQuizWorldRequest<PaginatedList<UserHistory>>
{
    public string? Search { get; set; } = null;

    [JsonIgnore]
    public Guid? UserId { get; set; }
}
