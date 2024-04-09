﻿using FluentValidation;

namespace QuizWorld.Application.MediatR.Identity.Commands.Login;

/// <summary>
/// The validator for the login command.
/// </summary>
public  class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
