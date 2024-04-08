using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    /// <param name="command"></param>
    /// <returns></returns>
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status409Conflict)]
    [SwaggerResponse(StatusCodes.Status201Created/*, Type = typeof(ProfileResponse)*/)]
    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupCommand command)
    {
        var response = await _sender.Send(command);

        return HandleResult(response);
    }
}
