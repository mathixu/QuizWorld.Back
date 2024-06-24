using FluentValidation;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Sessions.Commands.CreateSession;

public class CreateSessionCommandValidator : AbstractValidator<CreateSessionCommand>
{
    public CreateSessionCommandValidator(ICurrentUserService currentUserService)
    {
        RuleFor(x => x.QuizId)
            .NotEmpty()
            .WithMessage("Quiz id is required.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Session type is invalid.");

        if (!currentUserService.HasMinRole(Constants.MIN_TEACHER_ROLE))
        {
            RuleFor(x => x.Type)
                .Equal(SessionType.Singleplayer)
                .WithMessage("Only teachers and admins can create multiplayer sessions.");
        }
    }
}
