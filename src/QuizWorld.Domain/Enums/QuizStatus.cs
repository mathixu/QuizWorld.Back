namespace QuizWorld.Domain.Enums;

/// <summary>
/// Represents the status of a quiz.
/// </summary>
public enum QuizStatus
{
    /// <summary>
    /// Represents an unknown status.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Represents a draft status. (When questions are not yet generated)
    /// </summary>
    Draft = 1,

    /// <summary>
    /// Represents a pending status. (When questions are generated but not yet validated)
    /// </summary>
    Pending = 2,

    /// <summary>
    /// Represents an active status. (When questions are generated and validated)
    /// </summary>
    Active = 3,

    /// <summary>
    /// Represents an inactive status. (When questions are generated and invalidated)
    /// </summary>
    Inactive = 4
}
