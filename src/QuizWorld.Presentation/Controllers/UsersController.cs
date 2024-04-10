using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.MediatR.Users.Commands.AddPromotionToUser;
using QuizWorld.Application.MediatR.Users.Queries.GetProfile;
using QuizWorld.Application.MediatR.Users.Queries.GetUsers;
using QuizWorld.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace QuizWorld.Presentation.Controllers;

/// <summary>
/// The controller for the users operations.
/// </summary>
[Authorize(Policy = AvailableRoles.Teacher)]
[SwaggerResponse(StatusCodes.Status401Unauthorized)]
public class UsersController(ISender sender) : BaseApiController(sender)
{
    /// <summary>
    /// Gets the users.
    /// </summary>
    /// <param name="query">The query with the filter parameters.</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PaginatedList<ProfileResponse>))]
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query)
    {
        var response = await _sender.Send(query);

        return HandleResult(response);
    }

    /// <summary>
    /// Adds a promotion to a user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="promotionId">The promotion identifier.</param>
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProfileResponse))]
    [HttpPatch("{userId:guid}/promotions/{promotionId:guid}")]
    public async Task<IActionResult> AddPromotionToUser([FromRoute] Guid userId, [FromRoute] Guid promotionId)
    {
        var response = await _sender.Send(new AddPromotionToUserCommand(userId, promotionId));

        return HandleResult(response);
    }

    /// <summary>
    /// Gets the current user profile.
    /// </summary>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ProfileResponse))]
    [HttpGet("me")]
    [Authorize(Policy = AvailableRoles.Player)]
    public async Task<IActionResult> Me()
    {
        var response = await _sender.Send(new GetProfileQuery());

        return HandleResult(response);
    }
}
