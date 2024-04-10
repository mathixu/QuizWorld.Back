using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Promotions.Commands.CreatePromotion;

/// <summary>The command to create a promotion.</summary>
/// <param name="Name">The name of the promotion. Must not exceed 255 characters.</param>
public record CreatePromotionCommand(string Name) : IQuizWorldRequest<Promotion>;