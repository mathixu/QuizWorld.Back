namespace QuizWorld.Domain.Enums;

public enum SessionStatus
{
    /// <summary>
    /// Represents the status of the session when it is awaiting to start.
    /// </summary>
    Awaiting = 0,

    /// <summary>
    /// Represents the status of the session when it is started.
    /// </summary>
    Started = 1,

    /// <summary>
    /// Represents the status of the session when it is finished.
    /// </summary>
    Finished = 2,

    /// <summary>
    /// Represents the status of the session when it is cancelled.
    /// </summary>
    Cancelled = 3
}
