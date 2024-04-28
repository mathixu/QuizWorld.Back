namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents a skill weight entity.
/// </summary>
public class SkillWeight
{
    /// <summary>Represents the skill of the skill weight.</summary>
    public SkillTiny Skill { get; set; } = default!;

    /// <summary>Represents the weight of the skill.</summary>
    public int Weight { get; set; }
}
