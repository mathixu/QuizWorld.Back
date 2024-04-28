using QuizWorld.Domain.Common;
using QuizWorld.Domain.Enums;
using System.Text.Json.Serialization;

namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents a quiz entity.
/// </summary>
public class Quiz : BaseAuditableEntity
{
    /// <summary>Represents the title of the quiz.</summary>
    public string Name { get; set; } = default!;

    /// <summary>Represents the title of the quiz in normalized form.</summary>
    [JsonIgnore]
    public string NameNormalized { get; set; } = default!;

    /// <summary>Represents SkillWeights of the quiz.</summary>
    public List<SkillWeight> SkillWeights { get; set; } = default!;

    /// <summary>Represents the total number of questions in the quiz.</summary>
    public int TotalQuestions { get; set; }

    /// <summary>If the quiz has personalized questions for each user.</summary>
    public bool PersonalizedQuestions { get; set; }

    /// <summary>If we should display if the answer has multiple choices.</summary>
    public bool DisplayMultipleChoice { get; set; }

    /// <summary>Represents the user that created the quiz.</summary>
    public UserTiny CreatedBy { get; set; } = default!;

    /// <summary>Represents the status of the quiz.</summary>
    public Status Status { get; set; }

    /// <summary>Represents the file with context for the quiz.</summary>
    public QuizFile? File { get; set; } = default!;
}

/// <summary>
/// Represents a quiz entity with minimal information.
/// </summary>
public class QuizTiny : BaseEntity
{
    /// <summary>
    /// Represents the title of the quiz.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>Represents the total number of questions in the quiz.</summary>
    public int TotalQuestions { get; set; }

    /// <summary>
    /// Represents the skills of the quiz.
    /// </summary>
    public List<SkillTiny> Skills { get; set; } = default!;
}

/// <summary>Extension methods for the Quiz entity.</summary>
public static class QuizExtensions
{
    /// <summary>Converts a Quiz entity to a QuizTiny entity.</summary>
    /// <param name="quiz">The Quiz entity to convert.</param>
    /// <returns>The QuizTiny entity.</returns>
    public static QuizTiny ToTiny(this Quiz quiz)
    {
        return new QuizTiny
        {
            Id = quiz.Id,
            Name = quiz.Name,
            TotalQuestions = quiz.TotalQuestions,
            Skills = quiz.SkillWeights.Select(x => x.Skill).ToList()
        };
    }
}
