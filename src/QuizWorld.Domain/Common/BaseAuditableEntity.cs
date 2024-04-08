namespace QuizWorld.Domain.Common;

/// <summary>
/// Represents a base auditable entity.
/// </summary>
public class BaseAuditableEntity : BaseEntity
{
    /// <summary>
    /// The date and time when the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The date and time when the entity was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
