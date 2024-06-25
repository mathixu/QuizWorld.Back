using QuizWorld.Domain.Common;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents a session entity.
/// </summary>
public class Session : BaseAuditableEntity
{
    /// <summary>Represents the quizzes of the session.</summary>
    public QuizTiny Quiz { get; set; } = default!;

    /// <summary>Represents the code of the session.</summary>
    public string Code { get; set; } = default!;

    /// <summary>Represents the user that created the session.</summary>
    public UserTiny CreatedBy { get; set; } = default!;

    /// <summary>Represents the status of the session.</summary>
    public SessionStatus Status { get; set; }

    /// <summary>Represents the starting time of the session.</summary>
    public DateTime? StartingAt { get; set; } = default!;

    /// <summary>Represents the ending time of the session.</summary>
    public DateTime? EndingAt { get; set; } = default!;

    /// <summary>
    /// Represents the type of the session (Multiplayer, Singleplayer).
    /// </summary>
    public SessionType Type { get; set; } 
}

public class SessionTiny : BaseEntity
{
    /// <summary>Represents the code of the session.</summary>
    public string Code { get; set; } = default!;

    /// <summary>Represents the status of the session.</summary>
    public SessionStatus Status { get; set; }
}

public class SessionLight : BaseEntity
{
    /// <summary>Represents the code of the session.</summary>
    public string Code { get; set; } = default!;

    /// <summary>Represents the status of the session.</summary>
    public SessionStatus Status { get; set; }
    
    /// <summary>
    /// Represents the type of the session (Multiplayer, Singleplayer).
    /// </summary>
    public SessionType Type { get; set; }

    /// <summary>Represents the quizzes of the session.</summary>
    public QuizTiny Quiz { get; set; } = default!;


    /// <summary>Represents the starting time of the session.</summary>
    public DateTime? StartingAt { get; set; } = default!;

    /// <summary>Represents the ending time of the session.</summary>
    public DateTime? EndingAt { get; set; } = default!;
}

public static class SessionExtension
{
    public static SessionTiny ToTiny(this Session session)
    {
        return new SessionTiny
        {
            Id = session.Id,
            Code = session.Code,
            Status = session.Status
        };
    }

    public static SessionLight ToLight(this Session session)
    {
        return new SessionLight
        {
            Id = session.Id,
            Code = session.Code,
            Status = session.Status,
            Type = session.Type,
            Quiz = session.Quiz,
            StartingAt = session.StartingAt,
            EndingAt = session.EndingAt
        };
    }
}
