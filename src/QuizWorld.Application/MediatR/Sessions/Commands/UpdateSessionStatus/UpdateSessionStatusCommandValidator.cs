using FluentValidation;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Sessions.Commands.UpdateSessionStatus;

public class UpdateSessionStatusCommandValidator : AbstractValidator<UpdateSessionStatusCommand>
{
    public UpdateSessionStatusCommandValidator()
    {
        RuleFor(s => s.Status)
            .Must(s => Enum.IsDefined(typeof(SessionStatus), s))
            .WithMessage("Invalid session status.");

        RuleFor(s => s.Code) 
            .NotEmpty()
            .WithMessage("Session code is required.");
    }
}
