using FluentValidation;

namespace QuizWorld.Application.MediatR.Identity.Commands.Signup;

/// <summary>
/// The validator for the signup command.
/// </summary>
public  class SignupCommandValidator : AbstractValidator<SignupCommand>
{
    public SignupCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(255).WithMessage("First Name must not exceed 255 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(255).WithMessage("Last Name must not exceed 255 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(255).WithMessage("Password must not exceed 255 characters.");
    }
}
