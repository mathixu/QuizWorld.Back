using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;
using System.Text.Json.Serialization;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetUserHistories;

public class GetUserHistoriesQuery : PaginationQuery, IQuizWorldRequest<PaginatedList<UserHistory>>
{
    public string? Search { get; set; } = null;

    [JsonIgnore]
    public Guid SessionId { get; set; } 
}
