using QuizWorld.Domain.Common;

namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents an answer entity.
/// </summary>
public class Answer : BaseEntity
{
    /// <summary>
    /// Represents the text of the answer.
    /// </summary>
    public string Text { get; set; } = default!;

    /// <summary>
    /// Represents if the answer is correct.
    /// </summary>
    public bool? IsCorrect { get; set; } = default!;
}

public class AnswerTiny : BaseEntity
{
    /// <summary>
    /// Represents the text of the answer.
    /// </summary>
    public string Text { get; set; } = default!;
}

public static class AnswerExtensions
{
    public static AnswerTiny ToTiny(this Answer answer)
    {
        return new AnswerTiny
        {
            Id = answer.Id,
            Text = answer.Text
        };
    }
}