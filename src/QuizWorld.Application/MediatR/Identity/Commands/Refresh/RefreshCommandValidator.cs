using FluentValidation;

namespace QuizWorld.Application.MediatR.Identity.Commands.Refresh;

/// <summary>
/// The validator for the refresh command.
/// </summary>
public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token is required.");

        RuleFor(x => x.Token)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("Token is not in the correct format.");
    }
}
