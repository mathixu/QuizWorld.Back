using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Promotions.Queries.GetPromotions;

/// <summary>
/// Represents a query to get promotions.
/// </summary>
public class GetPromotionsQuery : PaginationQuery, IQuizWorldRequest<PaginatedList<PromotionTiny>>
{
    /// <summary>
    /// Search by name or id.
    /// </summary>
    public string? Search { get; set; } 
}
