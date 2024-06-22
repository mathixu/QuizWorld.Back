using QuizWorld.Domain.Common;

namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents a userAnswer entity.
/// </summary>
public class UserAnswer : BaseEntity
{
    /// <summary>Represents the id of the session.</summary>
    public Guid SessionId { get; set; }

    /// <summary>Represents the id of the quiz.</summary>
    public Guid QuizId { get; set; }

    /// <summary>Represents the id of the user.</summary>
    public Guid UserId { get; set; }

    /// <summary>Represents the id of the question.</summary>
    public Guid QuestionId { get; set; }

    /// <summary>Represents the list of answer id of the user.</summary>
    public List<Guid>? AnswerIds { get; set; }

    /// <summary>Represents true if the answer is correct, else false.</summary>
    public bool IsCorrect { get; set; }
}
