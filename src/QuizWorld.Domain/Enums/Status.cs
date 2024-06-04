namespace QuizWorld.Domain.Enums;

/// <summary>
/// Represents the status of an entity. (Pending = 0, Valid = 1, Invalid = 2)
/// </summary>
public enum Status
{
    /// <summary>
    ///  Represents a pending status.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Represents a valid status.
    /// </summary>
    Valid = 1,

    /// <summary>
    /// Represents an invalid status.
    /// </summary>
    Invalid = 2
}
