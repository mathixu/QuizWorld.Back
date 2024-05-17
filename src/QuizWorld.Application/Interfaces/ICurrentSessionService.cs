using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces;

public interface ICurrentSessionService
{
    /// <summary>
    /// Connects a user to the current session.
    /// </summary>
    void ConnectUser(string connectionId, User user);

    /// <summary>
    /// Disconnects a user from the current session.
    /// </summary>
    void DisconnectUser(string connectionId);

    /// <summary>
    /// Gets a user by connection id.
    /// </summary>
    User? GetUserByConnectionId(string connectionId);

    /// <summary>
    /// Adds a user session to the current session.
    /// </summary>
    void AddUserSession(User user, UserSession userSession);

    /// <summary>
    /// Removes a user session from the current session.
    /// </summary>
    void RemoveUserSession(User user);

    /// <summary>
    /// Gets a user session by user.
    /// </summary>
    UserSession? GetUserSessionByUser(User user);

    /// <summary>
    /// Session already has a teacher.
    /// </summary>
    bool AlreadyHaveTeacher(UserSession userSession, string code);

    /// <summary>
    /// User is already in a session.
    /// </summary>
    bool AlreadyInSession(string connectionId, User user);

    /// <summary>
    /// Disconnects an old user.
    /// </summary>
    void DisconnectOldUser(Guid userId);

    /// <summary>
    /// Gets online users for a session.
    /// </summary>
    List<UserTiny> GetOnlineUsers(string code);
}
