using QuizWorld.Domain.Common;
using QuizWorld.Domain.Enums;
using System.Text.Json.Serialization;

namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents a skill that can be associated with a quiz.
/// </summary>
public class Skill : BaseEntity
{
    /// <summary>Represents the name of the skill.</summary>
    public string Name { get; set; } = default!;

    /// <summary>Represents the normalized name of the skill.</summary>
    [JsonIgnore]
    public string NameNormalized { get; set; } = default!;
    
    /// <summary>Represents the source of the skill.</summary>
    public SkillSource Source { get; set; }
}

/// <summary>Represents a skill entity with minimal information.</summary>
public class SkillTiny : BaseEntity
{
    /// <summary>Represents the name of the skill.</summary>
    public string Name { get; set; } = default!;
}

/// <summary>
/// Extension methods for the Skill entity.
/// </summary>
public static class SkillExtensions
{
    /// <summary>Converts a Skill entity to a SkillTiny entity.</summary>
    /// <param name="skill">The Skill entity to convert.</param>
    /// <returns>The SkillTiny entity.</returns>
    public static SkillTiny ToTiny(this Skill skill)
    {
        return new SkillTiny
        {
            Id = skill.Id,
            Name = skill.Name
        };
    }
}
