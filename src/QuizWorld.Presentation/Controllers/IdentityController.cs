using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.MediatR.Identity.Commands.Login;
using QuizWorld.Application.MediatR.Identity.Commands.Refresh;
using QuizWorld.Application.MediatR.Identity.Commands.Signup;
using Swashbuckle.AspNetCore.Annotations;

namespace QuizWorld.Presentation.Controllers;

/// <summary>
/// The controller for identity operations.
/// </summary>
public class IdentityController(ISender sender) : BaseApiController(sender)
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="command">The DTO with the user's data.</param>
    [SwaggerResponse(StatusCodes.Status409Conflict)]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(ProfileAndTokensResponse))]
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupCommand command)
    {
        var response = await _sender.Send(command);

        return HandleResult(response);
    }

    /// <summary>
    /// Authenticates a user.
    /// </summary>
    /// <param name="command">The DTO with the user's email and password.</param>
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProfileAndTokensResponse))]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var response = await _sender.Send(command);

        return HandleResult(response);
    }

    /// <summary>
    /// Refreshes the user's tokens.
    /// </summary>
    /// <param name="command">The DTO with the token to refresh the user's tokens.</param>
    [SwaggerResponse(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TokensResponse))]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshCommand command)
    {
        var response = await _sender.Send(command);

        return HandleResult(response);
    }
}
