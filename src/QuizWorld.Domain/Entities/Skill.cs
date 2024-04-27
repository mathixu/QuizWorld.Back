using QuizWorld.Domain.Common;
using QuizWorld.Domain.Enums;
using System.Text.Json.Serialization;

namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents a skill that can be associated with a quiz.
/// </summary>
public class Skill : BaseEntity
{
    /// <summary>
    /// Represents the name of the skill.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Represents the normalized name of the skill.
    /// </summary>
    [JsonIgnore]
    public string NameNormalized { get; set; } = default!;
    
    /// <summary>
    /// Represents the source of the skill.
    /// </summary>
    public SkillSource Source { get; set; }
}
