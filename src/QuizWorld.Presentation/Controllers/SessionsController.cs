﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Sessions.Commands.CreateSession;
using QuizWorld.Application.MediatR.Sessions.Commands.UpdateSessionStatus;
using QuizWorld.Application.MediatR.Sessions.Queries.GetQuizResultDetails;
using QuizWorld.Application.MediatR.Sessions.Queries.GetSession;
using QuizWorld.Application.MediatR.Sessions.Queries.GetSessionById;
using QuizWorld.Application.MediatR.Sessions.Queries.GetSessionHistory;
using QuizWorld.Application.MediatR.Sessions.Queries.GetSessionResult;
using QuizWorld.Application.MediatR.Sessions.Queries.GetUserHistories;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;
using QuizWorld.Presentation.WebSockets;
using Swashbuckle.AspNetCore.Annotations;

namespace QuizWorld.Presentation.Controllers;

public class SessionsController(ISender sender, WebSocketService webSocketService) : BaseApiController(sender)
{
    private readonly WebSocketService _webSocketService = webSocketService;

    /// <summary>Creates a new session.</summary>
    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(Session))]
    [Authorize(Roles = Constants.MIN_STUDENT_ROLE)]
    public async Task<IActionResult> CreateSession([FromBody] CreateSessionCommand command)
        => await HandleCommand(command);

    /// <summary>Gets the status of a session.</summary>
    [HttpGet]
    [Authorize(Roles = Constants.MIN_STUDENT_ROLE)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Session))]
    public async Task<IActionResult> GetSession([FromQuery] string code)
        => await HandleCommand(new GetSessionQuery(code));

    /// <summary>Gets the status of a session.</summary>
    [HttpGet("{sessionId:guid}")]
    [Authorize(Roles = Constants.MIN_TEACHER_ROLE)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Session))]
    public async Task<IActionResult> GetSessionById([FromRoute] Guid sessionId)
        => await HandleCommand(new GetSessionByIdQuery(sessionId));

    /// <summary>
    /// Updates the status of a session.
    /// </summary>
    [HttpPut("{code}/status")]
    [Authorize(Roles = Constants.MIN_STUDENT_ROLE)]
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
    {
        var result = await _sender.Send(new GetSessionResultQuery(code));

        if (result.IsSuccessful)
        {
            await _webSocketService.HandleAction(WebSocketAction.UserFinishedQuiz);
        }

        return HandleResult(result);
    }

    [HttpGet("{sessionId:guid}/users/{userId:guid}/answers")]
    [Authorize(Roles = Constants.MIN_STUDENT_ROLE)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<UserAnswer>))]
    public async Task<IActionResult> GetSessionUserAnswers([FromRoute] Guid sessionId, [FromRoute]Guid userId)
            => await HandleCommand(new GetSessionResultDetailsQuery(sessionId, userId));

    [HttpGet("history")]
    [Authorize(Roles = Constants.MIN_STUDENT_ROLE)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PaginatedList<SessionLight>))]
    public async Task<IActionResult> GetSessionHistory([FromQuery] GetSessionHistoryQuery query)
        => await HandleCommand(query);

    [HttpGet("{sessionId:guid}/history")]
    [Authorize(Roles = Constants.MIN_STUDENT_ROLE)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PaginatedList<UserHistory>))]
    public async Task<IActionResult> GetUserHistoriesBySession([FromRoute] Guid sessionId, [FromQuery] GetUserHistoriesQuery query)
    {
        query.SessionId = sessionId;

        return await HandleCommand(query);
    }
}
