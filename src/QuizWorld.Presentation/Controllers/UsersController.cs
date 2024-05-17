using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Users.Queries.GetCurrentUser;
using QuizWorld.Application.MediatR.Users.Queries.SearchUsers;
using QuizWorld.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace QuizWorld.Presentation.Controllers;

public class UsersController(ISender sender) : BaseApiController(sender)
{
    /// <summary>Gets the current user.</summary>
    [HttpGet("~/profile")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(User))]
    public async Task<IActionResult> GetCurrentUser()
        => await HandleCommand(new GetCurrentUserQuery());

    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PaginatedList<UserTiny>))]
    /// <summary>Searches for users.</summary>
    [HttpGet]
    public async Task<IActionResult> SearchUsers([FromQuery] SearchUsersQuery query)
        => await HandleCommand(query);
}
