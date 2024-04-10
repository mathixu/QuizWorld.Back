using MediatR;
using QuizWorld.Application.MediatR.Common;

namespace QuizWorld.Application.MediatR.Promotions.Commands.DeletePromotion;

/// <summary>
/// Represents a command to delete a promotion.
/// </summary>
/// <param name="PromotionId">The identifier of the promotion to delete.</param>
public record DeletePromotionCommand(Guid PromotionId) : IQuizWorldRequest<Unit>;