using FluentValidation;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Admin.Commands.PromoteUser;

/// <summary>
/// Represents the validator for the promote user command.
/// </summary>
public class PromoteUserCommandValidator : AbstractValidator<PromoteUserCommand>
{
    public PromoteUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("The user's id is required.");

        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("The role is required.");

        RuleFor(x => x.Role)
            .Must(x =>x == AvailableRoles.Admin || x == AvailableRoles.Teacher || x == AvailableRoles.Player)
            .WithMessage("The role must be either 'Admin', 'Teacher', or 'Player'.");
    }
}
