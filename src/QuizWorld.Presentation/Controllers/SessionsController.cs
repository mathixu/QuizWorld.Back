using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.MediatR.Sessions.Commands.CreateSession;
using QuizWorld.Application.MediatR.Sessions.Queries.GetSessionStatus;

namespace QuizWorld.Presentation.Controllers;

public class SessionsController(ISender sender) : BaseApiController(sender)
{
    /// <summary>Creates a new session.</summary>
    [HttpPost]
    [Authorize(Roles = Constants.MIN_TEACHER_ROLE)]
    public async Task<IActionResult> CreateSession([FromBody] CreateSessionCommand command)
        => await HandleCommand(command);

    /// <summary>Gets the status of a session.</summary>
    [HttpGet("status")]
    public async Task<IActionResult> GetSessionStatus([FromQuery] string code)
        => await HandleCommand(new GetSessionStatusQuery(code));
}
