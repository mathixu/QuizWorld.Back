using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.MediatR.Sessions.Commands.CreateSession;
using QuizWorld.Application.MediatR.Sessions.Commands.UpdateSessionStatus;
using QuizWorld.Application.MediatR.Sessions.Queries.GetSession;
using QuizWorld.Application.MediatR.Sessions.Queries.GetSessionResult;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;
using QuizWorld.Presentation.WebSockets;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace QuizWorld.Presentation.Controllers;

public class SessionsController(ISender sender, IHubContext<QuizSessionHub> hubContext) : BaseApiController(sender)
{
    private readonly IHubContext<QuizSessionHub> _hubContext = hubContext;

    /// <summary>Creates a new session.</summary>
    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(Session))]
    [Authorize(Roles = Constants.MIN_TEACHER_ROLE)]
    public async Task<IActionResult> CreateSession([FromBody] CreateSessionCommand command)
        => await HandleCommand(command);

    /// <summary>Gets the status of a session.</summary>
    [HttpGet]
    [Authorize(Roles = Constants.MIN_STUDENT_ROLE)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Session))]
    public async Task<IActionResult> GetSession([FromQuery] string code)
        => await HandleCommand(new GetSessionQuery(code));

    /// <summary>
    /// Updates the status of a session.
    /// </summary>
    [HttpPut("{code}/status")]
    [Authorize(Roles = Constants.MIN_TEACHER_ROLE)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Session))]
    public async Task<IActionResult> UpdateSessionStatus([FromRoute] string code, [FromBody] UpdateSessionStatusCommand command)
    {
        command.Code = code;

        var response = await _sender.Send(command);

        if (response.IsSuccessful)
        {
            if (response.Data.Status == SessionStatus.Started)
            {
                // Send a notification to the students that the session has started
                await _hubContext.Clients.Group(code).SendAsync("ReceiveMessage", JsonSerializer.Serialize(new {message = "session_started" }));
            }
            else if (response.Data.Status == SessionStatus.Finished)
            {
                // Send a notification to the students that the session has finished
                await _hubContext.Clients.Group(code).SendAsync("ReceiveMessage", JsonSerializer.Serialize(new {message = "session_finished" }));
            }
        }

        return HandleResult(response);
    }

    [HttpGet("{code}/result")]
    [Authorize(Roles = Constants.MIN_STUDENT_ROLE)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserSessionResult))]
    public async Task<IActionResult> GetSessionResult(string code)
        => await HandleCommand(new GetSessionResultQuery(code));
}
