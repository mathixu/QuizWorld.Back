namespace QuizWorld.Application.Common.Models;

public enum WebSocketAction
{
    /// <summary>
    /// Represents no action.
    /// </summary>
    None = 0,

    /// <summary>
    /// Represents the user started a quiz.
    /// </summary>
    UserStartedQuiz = 1,

    /// <summary>
    /// Represents the user finished a quiz.
    /// </summary>
    UserFinishedQuiz = 2, 

    /// <summary>
    /// Represents the start of a session.
    /// </summary>
    StartSession = 3,

    /// <summary>
    /// Represents the stop of a session.
    /// </summary>
    StopSession = 4,
}
