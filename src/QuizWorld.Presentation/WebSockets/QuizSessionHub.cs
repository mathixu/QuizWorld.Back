using Microsoft.AspNetCore.SignalR;

namespace QuizWorld.Presentation.WebSockets;

public class QuizSessionHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var header = Context.GetHttpContext();

        var accessToken = header.Request.Headers["Authorization"];

        await base.OnConnectedAsync();
    }
}
