using Microsoft.AspNetCore.SignalR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;
using System.Text.Json;

namespace QuizWorld.Presentation.WebSockets;

public class QuizSessionHub(ICurrentUserService currentUserService, ISessionService sessionService, ICurrentSessionService currentSessionService) : Hub
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly ISessionService _sessionService = sessionService;
    private readonly ICurrentSessionService _currentSessionService = currentSessionService;

    public override async Task OnConnectedAsync()
    {
        try
        {
            var user = _currentUserService.ExtractUser(Context.User);

            _currentSessionService.ConnectUser(Context.ConnectionId, user);

            await base.OnConnectedAsync();
        }
        catch { }
    }

    public async Task JoinSession(string code)
    {
        try
        {
            var user = _currentSessionService.GetUserByConnectionId(Context.ConnectionId);

            if (user is null)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "You are not authenticated.");

                await DisconnectUser(Context.ConnectionId);

                return;
            }

            if (_currentSessionService.AlreadyInSession(Context.ConnectionId, user))
            {
                _currentSessionService.RemoveUserSession(user);

                await _sessionService.ChangeUserSessionStatus(Context.ConnectionId, UserSessionStatus.DisconnectedByUser);

                _currentSessionService.DisconnectOldUser(user.Id);
            }

            var sessionStatus = await _sessionService.GetSessionByCode(code);

            switch(sessionStatus.Status)
            {
                case SessionStatus.None:
                    await Clients.Caller.SendAsync("ReceiveMessage", "The session does not exist.");
                    await DisconnectUser(Context.ConnectionId);
                    return;

                case SessionStatus.Started:
                    await Clients.Caller.SendAsync("ReceiveMessage", "The session has already started.");
                    await DisconnectUser(Context.ConnectionId);
                    return;

                case SessionStatus.Finished:
                    await Clients.Caller.SendAsync("ReceiveMessage", "The session has already finished.");
                    await DisconnectUser(Context.ConnectionId);
                    return;
            }

            var userSession = await _sessionService.AddUserSession(code, Context.ConnectionId, user);

            _currentSessionService.AddUserSession(user, userSession);

            await Groups.AddToGroupAsync(Context.ConnectionId, code);

            await Clients.Group(code).SendAsync("ReceiveMessage", BuildOnlineUserResponse(userSession));

            await Clients.Caller.SendAsync("ReceiveMessage", "You have joined the session successfully.");
        }
        catch (BadRequestException ex)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", ex.Message);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", $"An error occurred while joining the session. {ex.Message}");
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            var user = _currentSessionService.GetUserByConnectionId(Context.ConnectionId);

            if (user is not null)
                await DisconnectUser(Context.ConnectionId, exception is null);

            await base.OnDisconnectedAsync(exception);
        }
        catch
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "An error occurred while disconnecting from the session.");
        }
    }

    private async Task DisconnectUser(string connectionId, bool isError = false)
    {
        try
        {
            var user = _currentSessionService.GetUserByConnectionId(connectionId);
            if (user is null)
            {
                Context.Abort();

                return;
            }

            var userSession = _currentSessionService.GetUserSessionByUser(user);

            if (userSession is not null)
            {
                await Groups.RemoveFromGroupAsync(connectionId, userSession.Session.Code);

                _currentSessionService.RemoveUserSession(user);
                
                await _sessionService.ChangeUserSessionStatus(connectionId, isError ? UserSessionStatus.DisconnectedWithError : UserSessionStatus.DisconnectedByUser);
            }

            _currentSessionService.DisconnectUser(connectionId);

            Context.Abort();
        }
        catch
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "An error occurred while disconnecting from the session.");
        }
    }

    private string BuildOnlineUserResponse(UserSession userSession)
    {
        var usersTinies = _currentSessionService.GetOnlineUsers(userSession.Session.Code);

        return JsonSerializer.Serialize(new { users = usersTinies, sessionId = userSession.Session.Id},
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );
    }
}
