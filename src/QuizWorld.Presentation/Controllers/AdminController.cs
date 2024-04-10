using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.MediatR.Admin.Commands.PromoteUser;
using QuizWorld.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace QuizWorld.Presentation.Controllers;

/// <summary>
/// The controller for the admin operations.
/// </summary>
[Authorize(Roles = AvailableRoles.Admin)]
[SwaggerResponse(StatusCodes.Status401Unauthorized)]
public class AdminController(ISender sender) : BaseApiController(sender)
{

    /// <summary>
    /// Promotes a user to a new role.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="command">The DTO with the new role.</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProfileResponse))]
    [HttpPut("users/{userId:guid}/promote")]
    public async Task<IActionResult> PromoteUser([FromRoute] Guid userId, [FromBody] PromoteUserCommand command)
    {
        command.UserId = userId;

        var response = await _sender.Send(command);

        return HandleResult(response);
    }
}
