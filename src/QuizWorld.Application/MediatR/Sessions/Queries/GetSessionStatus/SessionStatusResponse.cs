using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetSessionStatus;

public class SessionStatusResponse
{
    /// <summary>Represents the status of the session.</summary>
    public SessionStatus Status { get; set; }

    /// <summary>Represents if the user is authorized to access the session.
    /// ( session not started and user is in the quiz's users list )</summary>
    public bool IsAuthorized { get; set; }
}
