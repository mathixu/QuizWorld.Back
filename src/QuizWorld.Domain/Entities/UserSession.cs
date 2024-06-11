using QuizWorld.Domain.Common;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Domain.Entities;

public class UserSession(UserTiny userTiny, SessionTiny session, string connectionId, bool isTeacher = false) : BaseEntity
{
    /// <summary>Represents the id of the user session.</summary>
    public UserTiny User { get; set; } = userTiny;

    /// <summary>Represents the connection id of the user.</summary>
    public string ConnectionId { get; set; } = connectionId;

    /// <summary>Represents the session of the user.</summary>
    public SessionTiny Session { get; set; } = session;

    /// <summary>Represents the connected at date time.</summary>
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Represents the disconnected at date time.</summary>
    public DateTime? DisconnectedAt { get; set; }

    /// <summary>Represents the status of the user session.</summary>
    public UserSessionStatus Status { get; set; } = UserSessionStatus.Connected;

    /// <summary>Represents whether the user is a teacher of the session.</summary>
    public bool IsTeacher { get; set; } = isTeacher;

    /// <summary>
    /// Represents the identifiers of the questions to be answered by the user.
    /// </summary>
    public List<Guid> QuestionIds { get; set; } = [];

    /// <summary>
    /// Represents the result of the session.
    /// </summary>
    public UserSessionResult? Result { get; set; } = null;
}
