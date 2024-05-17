using FluentValidation;

namespace QuizWorld.Application.MediatR.Sessions.Commands.CreateSession;

public class CreateSessionCommandValidator : AbstractValidator<CreateSessionCommand>
{
    public CreateSessionCommandValidator()
    {
        RuleFor(x => x.QuizIds)
            .NotEmpty()
            .WithMessage("The session must have at least one quiz.");

        RuleFor(x => x.QuizIds)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("The session must have unique quizzes.");
    }
}
