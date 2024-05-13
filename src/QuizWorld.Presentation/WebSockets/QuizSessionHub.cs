using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Services;
using System.Security.Claims;

namespace QuizWorld.Presentation.WebSockets;

public class QuizSessionHub(ICurrentUserService currentUserService) : Hub
{
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public Dictionary<string, User> ConnectedUsers { get; } = new();

    [Authorize]
    public override async Task OnConnectedAsync()
    {
        var user = _currentUserService.User;

        if (user is not null)
        {
            ConnectedUsers.Add(Context.ConnectionId, user);
        }

        await base.OnConnectedAsync();
    }
}
