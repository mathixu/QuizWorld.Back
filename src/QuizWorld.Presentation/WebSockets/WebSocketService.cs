using Microsoft.AspNetCore.SignalR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;
using System.Text.Json;

namespace QuizWorld.Presentation.WebSockets;

public class WebSocketService(IHubContext<QuizSessionHub> hubContext, ICurrentSessionService currentSessionService, ICurrentUserService currentUserService)
{
    private readonly IHubContext<QuizSessionHub> _hubContext = hubContext;
    private readonly ICurrentSessionService _currentSessionService = currentSessionService;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    /// <summary>
    /// Handles the action sent by the client.
    /// </summary>
    public async Task HandleAction(WebSocketAction action)
    {
        var user = _currentUserService.User
                        ?? throw new UnauthorizedAccessException("You are not connected.");

        var sessionId = _currentSessionService.GetUserSessionByUserId(user.Id).Session.Id;

        switch(action)
        {
            case WebSocketAction.StartSession:
                await StartSession(sessionId);
                break;
            case WebSocketAction.StopSession:
                await StopSession(sessionId);
                break;
            case WebSocketAction.UserStartedQuiz:
                await SendOnlineUserToTeacher(sessionId, UserStatus.InProgress);
                break;
            case WebSocketAction.UserFinishedQuiz:
                await SendOnlineUserToTeacher(sessionId, UserStatus.Done);
                break;
        }
    }

    private async Task StartSession(Guid sessionId)
    {
        var session = _currentSessionService.GetTeacherBySessionId(sessionId);

        var code = session.Session.Code;

        await _hubContext.Clients.Group(code).SendAsync("ReceiveMessage", JsonSerializer.Serialize(new { message = "session_started" }));
    }

    private async Task StopSession(Guid sessionId)
    {
        var teacher = _currentSessionService.GetTeacherBySessionId(sessionId);

        var code = teacher.Session.Code;

        var onlineUsers = _currentSessionService.GetOnlineUsers(code);

        foreach (var user in onlineUsers)
        {
            _currentSessionService.ChangeUserStatus(user.Id, UserStatus.Done);
        }

        await _hubContext.Clients.Group(code).SendAsync("ReceiveMessage", JsonSerializer.Serialize(new { message = "session_finished" }));

        var users = JsonSerializer.Serialize(new { users = onlineUsers, sessionId },
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await _hubContext.Clients.Client(teacher.ConnectionId).SendAsync("ReceiveMessage", users);
    }

    private async Task SendOnlineUserToTeacher(Guid sessionId, UserStatus? status)
    {
        var teacher = _currentSessionService.GetTeacherBySessionId(sessionId)
        ?? throw new UnauthorizedAccessException("The session does not exist.");

        if (status.HasValue)
        {
            var currentUserId = _currentUserService.UserId
                ?? throw new UnauthorizedAccessException("You are not conntected.");

            _currentSessionService.ChangeUserStatus(currentUserId, status.Value);
        }

        var onlineUsers = _currentSessionService.GetOnlineUsers(teacher.Session.Code);

        var users = JsonSerializer.Serialize(new { users = onlineUsers, sessionId },
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await _hubContext.Clients.Client(teacher.ConnectionId).SendAsync("ReceiveMessage", users);
    }
}
