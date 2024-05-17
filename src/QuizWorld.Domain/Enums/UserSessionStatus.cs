namespace QuizWorld.Domain.Enums;

public enum UserSessionStatus
{
    /// <summary>
    /// Represents the user is connected.
    /// </summary>
    Connected = 1,

    /// <summary>
    /// Represents the user is disconnected with an error.
    /// </summary>
    DisconnectedWithError = 2,

    /// <summary>
    /// Represents the user is disconnected by the user.
    /// </summary>
    DisconnectedByUser = 3,
}
