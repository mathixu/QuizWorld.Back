namespace QuizWorld.Domain.Enums;

public enum SessionStatus
{
    /// <summary>
    /// Represents the status of the session when it is none.
    /// </summary>
    None = 0,

    /// <summary>
    /// Represents the status of the session when it is awaiting to start.
    /// </summary>
    Awaiting = 1,

    /// <summary>
    /// Represents the status of the session when it is started.
    /// </summary>
    Started = 2,

    /// <summary>
    /// Represents the status of the session when it is finished.
    /// </summary>
    Finished = 3,

    /// <summary>
    /// Represents the status of the session when it is cancelled.
    /// </summary>
    Cancelled = 4,
}
