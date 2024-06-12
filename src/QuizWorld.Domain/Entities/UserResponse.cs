using QuizWorld.Domain.Common;

namespace QuizWorld.Domain.Entities;

public class UserResponse : BaseAuditableEntity
{
    /// <summary>Represents the user of the user response.</summary>
    public UserTiny User { get; set; } = default!;

    /// <summary>
    /// Represents the quizId of the user response.
    /// </summary>
    public Guid QuizId { get; set; }

    /// <summary>
    /// Represents the questionId of the user response.
    /// </summary>
    public Guid SkillId { get; set; }

    /// <summary>
    /// Represents the question of the user response.
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

    /// <summary>
    /// Represents whether the last response is correct.
    /// </summary>
    public bool LastResponseIsCorrect { get; set; }
}