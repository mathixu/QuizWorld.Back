using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Sessions.Commands.CreateSession;

public class CreateSessionCommand : IQuizWorldRequest<Session>
{
    /// <summary>
    /// Represents the quizzes of the session.
    /// </summary>
    public List<Guid> QuizIds { get; set; } = default!;
}
