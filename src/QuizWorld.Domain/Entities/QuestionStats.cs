using QuizWorld.Domain.Common;

namespace QuizWorld.Domain.Entities;

public class QuestionStats : BaseEntity
{
    /// <summary>
    /// Represents the question of the question stats.
    /// </summary>
    public QuestionMinimal Question { get; set; } = default!;

    /// <summary>
    /// Represents the number of attempts of the question.
    /// </summary>
    public int Attempts { get; set; }

    /// <summary>
    /// Represents the success rate of the question.
    /// </summary>
    public double SuccessRate { get; set; } 
}