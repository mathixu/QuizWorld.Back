using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetSessionStatus;

public class SessionStatusResponse
{
    /// <summary>Represents the status of the session.</summary>
    public SessionStatus Status { get; set; }
}
