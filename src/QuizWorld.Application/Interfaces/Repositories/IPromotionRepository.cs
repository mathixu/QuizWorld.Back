using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Promotions.Queries.GetPromotions;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces.Repositories;

/// <summary>
/// The repository for promotions.
/// </summary>
public interface IPromotionRepository
{
    /// <summary>
    /// Creates a promotion asynchronously.
    /// </summary>
    /// <param name="promotion">The promotion to create.</param>
    /// <returns>The result of the operation.</returns>
    Task<bool> AddAsync(Promotion promotion);

    /// <summary>
    /// Deletes a promotion asynchronously.
    /// </summary>
    /// <param name="promotionId">The ID of the promotion to delete.</param>
    /// <returns>The result of the operation.</returns>
    Task<bool> DeleteAsync(Guid promotionId);

    /// <summary>Get a promotion by ID asynchronously.</summary>
    /// <param name="promotionId">The ID of the promotion to get.</param>
    /// <returns>The promotion with the given ID or null if not found.</returns>
    Task<Promotion?> GetByIdAsync(Guid promotionId);

    /// <summary>xGets promotions asynchronously.</summary>
    /// <param name="query">The filters to apply to the query.</param>
    /// <returns>The paginated list of promotions.</returns>
    Task<PaginatedList<Promotion>> GetPromotionsAsync(GetPromotionsQuery query);
}
