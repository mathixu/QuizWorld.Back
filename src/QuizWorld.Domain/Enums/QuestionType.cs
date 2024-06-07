namespace QuizWorld.Domain.Enums;

/// <summary>
/// Represents the type of a question. (SimpleChoice = 1, MultipleChoice = 2, Combinaison = 3)
/// </summary>
public enum QuestionType
{
    /// <summary>
    /// Represents an unknown question type.
    /// </summary>
    Unknown = 0,

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