using QuizWorld.Domain.Common;

namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents a promotion entity.
/// </summary>
public class Promotion : BaseAuditableEntity
{
    /// <summary>
    /// The name of the promotion.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The year of graduation. (e.g. 2023)
    /// </summary>
    public int GraduationYear { get; set; }
}

/// <summary>
/// A tiny version of the Promotion entity.
/// </summary>
public class PromotionTiny
{
    /// <summary>
    /// The unique identifier of the promotion.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the promotion.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The year of graduation. (e.g. 2023)
    /// </summary>
    public int GraduationYear { get; set; }
}

public static class PromotionExtensions
{
    /// <summary>
    /// Converts a Promotion entity to a PromotionTiny entity.
    /// </summary>
    /// <param name="promotion">The Promotion entity to convert.</param>
    /// <returns>The PromotionTiny entity.</returns>
    public static PromotionTiny ToTiny(this Promotion promotion)
    {
        return new PromotionTiny
        {
            Id = promotion.Id,
            Name = promotion.Name,
            GraduationYear = promotion.GraduationYear
        };
    }
}
