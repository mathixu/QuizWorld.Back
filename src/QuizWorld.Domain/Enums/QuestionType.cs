namespace QuizWorld.Domain.Enums;

/// <summary>
/// Represents the type of a question.
/// </summary>
public enum QuestionType
{
    /// <summary>
    /// Represents a simple choice question.
    /// </summary>
    SimpleChoice = 1,

    /// <summary>
    /// Represents a multiple choice question.
    /// </summary>
    MultipleChoice = 2,

    /// <summary>
    /// Represents a combinaison question.
    /// </summary>
    Combinaison = 3
}
