using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Services;

public class CurrentSessionService : ICurrentSessionService
{
    private readonly Dictionary<string, User> _connections = [];
    private readonly Dictionary<User, UserSession> _userSessions = [];

    public void ConnectUser(string connectionId, User user)
    {
        _connections.Add(connectionId, user);
    }

    public void DisconnectUser(string connectionId)
    {
        _connections.Remove(connectionId);
    }

    public User? GetUserByConnectionId(string connectionId)
    {
        return _connections.TryGetValue(connectionId, out User? user) ? user : null;
    }

    public void AddUserSession(User user, UserSession userSession)
    {
        _userSessions.Add(user, userSession);
    }

    public void RemoveUserSession(User user)
    {
        _userSessions.Remove(user);
    }

    public UserSession? GetUserSessionByUser(User user)
    {
        return _userSessions.FirstOrDefault(x => x.Key.Id == user.Id).Value;
    }

    public bool AlreadyHaveTeacher(UserSession userSession, string code)
    {
        return userSession.IsTeacher && _userSessions.Any(x => x.Value.Session.Code == code && x.Value.IsTeacher);
    }

    public bool AlreadyInSession(string connectionId, User user)
    {
        return _userSessions.Any(x => x.Value.ConnectionId == connectionId || x.Key.Id == user.Id);
    }

    public List<UserTiny> GetOnlineUsers(string code)
    {
        return _userSessions
            .Where(x => x.Value.Session.Code == code)
            .Select(x => x.Key.ToTiny())
            .ToList();
    }

    public void DisconnectOldUser(Guid userId)
    {
        var user = _userSessions.FirstOrDefault(x => x.Key.Id == userId).Key;

        if (user is not null)
        {
            _userSessions.Remove(user);

            // remove all from connection
            var connections = _connections.Where(x => x.Value.Id == userId).ToList();

            foreach (var connection in connections)
            {
                _connections.Remove(connection.Key);
            }
        }
    }
}
