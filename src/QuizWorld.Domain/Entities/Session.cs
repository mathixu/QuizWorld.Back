using QuizWorld.Domain.Common;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents a session entity.
/// </summary>
public class Session : BaseAuditableEntity
{
    /// <summary>Represents the quizzes of the session.</summary>
    public List<QuizTiny> Quizzes { get; set; } = [];

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
}