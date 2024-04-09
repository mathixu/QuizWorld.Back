using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.MediatR.Users.Queries.GetUsers;
using Swashbuckle.AspNetCore.Annotations;

namespace QuizWorld.Presentation.Controllers;

/// <summary>
/// The controller for the users operations.
/// </summary>
[Authorize]
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
}
