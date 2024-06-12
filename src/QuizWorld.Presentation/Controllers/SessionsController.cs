﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Common.Models;
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

public class SessionsController(ISender sender, WebSocketService webSocketService) : BaseApiController(sender)
{
    private readonly WebSocketService _webSocketService = webSocketService;

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
            var action = response.Data.Status == SessionStatus.Started ? WebSocketAction.StartSession 
                                    : response.Data.Status == SessionStatus.Finished ? WebSocketAction.StopSession 
                                    : WebSocketAction.None;

            await webSocketService.HandleAction(action);
        }

        return HandleResult(response);
    }

    /// <summary>Gets the result of a session.</summary>
    [HttpGet("{code}/result")]
    [Authorize(Roles = Constants.MIN_STUDENT_ROLE)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserSessionResult))]
    public async Task<IActionResult> GetSessionResult(string code)
        => await HandleCommand(new GetSessionResultQuery(code));
}
