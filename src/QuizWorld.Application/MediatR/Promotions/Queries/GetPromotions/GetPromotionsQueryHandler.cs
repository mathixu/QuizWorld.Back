using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Promotions.Queries.GetPromotions;

/// <summary>
/// Represents a query handler to get promotions.
/// </summary>
/// <param name="promotionRepository"></param>
public class GetPromotionsQueryHandler(IPromotionRepository promotionRepository) : IRequestHandler<GetPromotionsQuery, QuizWorldResponse<PaginatedList<PromotionTiny>>>
{
    private readonly IPromotionRepository _promotionRepository = promotionRepository;

    public async Task<QuizWorldResponse<PaginatedList<PromotionTiny>>> Handle(GetPromotionsQuery request, CancellationToken cancellationToken)
    {
        var promotions = await _promotionRepository.GetPromotionsAsync(request);

        var promotionsTiny = promotions.Map<PromotionTiny>(x => x.ToTiny());

        return QuizWorldResponse<PaginatedList<PromotionTiny>>.Success(promotionsTiny, 200);
    }
}
