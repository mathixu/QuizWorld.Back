using QuizWorld.Domain.Common;
using System.Text.Json.Serialization;

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
    /// The normalized name of the promotion.
    /// </summary>
    [JsonIgnore]
    public string NameNormalized { get; set; } = default!;
}

/// <summary>
/// A tiny version of the Promotion entity.
/// </summary>
public class PromotionTiny : BaseEntity
{
    /// <summary>
    /// The name of the promotion.
    /// </summary>
    public string Name { get; set; } = default!;
}

/// <summary>
/// Extension methods for the Promotion entity.
/// </summary>
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
            Name = promotion.Name
        };
    }
}
