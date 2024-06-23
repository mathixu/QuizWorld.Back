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

        var currentUserRoles = currentUserService.UserRoles
            ?? throw new UnauthorizedAccessException();

        if (!currentUserRoles.Contains(Constants.TEACHER_ROLE) && !currentUserRoles.Contains(Constants.ADMIN_ROLE))
        {
            RuleFor(x => x.Type)
                .Equal(SessionType.Singleplayer)
                .WithMessage("Only teachers and admins can create multiplayer sessions.");
        }
    }
}
