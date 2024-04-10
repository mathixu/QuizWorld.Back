using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Promotions.Commands.DeletePromotion;

/// <summary>
/// Represents a command handler to delete a promotion.
/// </summary>
public class DeletePromotionCommandHandler(IPromotionRepository promotionRepository, IUserRepository userRepository) : IRequestHandler<DeletePromotionCommand, QuizWorldResponse<Unit>>
{
    private readonly IPromotionRepository _promotionRepository = promotionRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<QuizWorldResponse<Unit>> Handle(DeletePromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = await _promotionRepository.GetByIdAsync(request.PromotionId)
            ?? throw new NotFoundException(nameof(Promotion), request.PromotionId);

        var result = await _promotionRepository.DeleteAsync(request.PromotionId);

        if (!result)
            return QuizWorldResponse<Unit>.Failure("Failed to delete promotion.");

        await _userRepository.RemovePromotionAsync(promotion.Id);

        return QuizWorldResponse<Unit>.Success(Unit.Value, 204);
    }
}
