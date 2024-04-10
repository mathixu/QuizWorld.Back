using AutoMapper;
using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Promotions.Commands.CreatePromotion;

/// <summary>
/// The command handler to create a promotion.
/// </summary>
public class CreatePromotionCommandHandler(IPromotionRepository promotionRepository, IMapper mapper) : IRequestHandler<CreatePromotionCommand, QuizWorldResponse<Promotion>>
{
    private readonly IPromotionRepository _promotionRepository = promotionRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<QuizWorldResponse<Promotion>> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = _mapper.Map<Promotion>(request);

        await _promotionRepository.AddAsync(promotion);

        return QuizWorldResponse<Promotion>.Success(promotion, 201);
    }
}
