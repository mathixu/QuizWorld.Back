using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.MediatR.Common;

namespace QuizWorld.Application.MediatR.Users.Commands.AddPromotionToUser;

/// <summary>Represents the command to add a promotion to a user.</summary>
/// <param name="UserId">The ID of the user to add the promotion to.</param>
/// <param name="PromotionId">The ID of the promotion to add to the user.</param>
public record AddPromotionToUserCommand(Guid UserId, Guid PromotionId) : IQuizWorldRequest<ProfileResponse>;
