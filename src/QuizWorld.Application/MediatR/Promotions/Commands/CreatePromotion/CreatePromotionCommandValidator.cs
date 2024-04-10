using FluentValidation;

namespace QuizWorld.Application.MediatR.Promotions.Commands.CreatePromotion;

/// <summary>
/// The validator for the CreatePromotionCommand.
/// </summary>
public class CreatePromotionCommandValidator : AbstractValidator<CreatePromotionCommand>
{
    public CreatePromotionCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(255)
            .WithMessage("Name must not exceed 255 characters.");
    }
}
