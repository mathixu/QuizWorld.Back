namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents the result of a session.
/// </summary>
public class UserSessionResult
{
    /// <summary>
    /// The note of the session.
    /// </summary>
    public int? Note { get; set; } = null; 

    /// <summary>
    /// The total number of questions.
    /// </summary>
    public int TotalQuestions { get; set; }

    /// <summary>
    /// The number of questions answered.
    /// </summary>
    public int QuestionsAnswered { get; set; }

    /// <summary>
    /// The worked skills and their percentage of mastery.
    /// </summary>
    public List<SkillWeightExtended>? SkillWeights { get; set; } = null;

    /// <summary>
    /// The date and time when the session started.
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// The date and time when the session ended.
    /// </summary>
    public DateTime? EndedAt { get; set; }
}

public class SkillWeightExtended : SkillWeight
{
    /// <summary>
    /// The number of attempts.
    /// </summary>
    public int Attempts { get; set; }
}
