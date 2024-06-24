using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Sessions.Commands.CreateSession;

public class CreateSessionCommand : IQuizWorldRequest<Session>
{
    /// <summary>
    /// Represents the quiz id of the session.
    /// </summary>
    public Guid QuizId { get; set; } = default!;

    public SessionType Type { get; set; }
}
