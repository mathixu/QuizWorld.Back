﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Users.Queries.GetCurrentUser;
using QuizWorld.Application.MediatR.Users.Queries.GetUserById;
using QuizWorld.Application.MediatR.Users.Queries.SearchHistory;
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

    /// <summary>Searches for users.</summary>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PaginatedList<UserTiny>))]
    [HttpGet]
    public async Task<IActionResult> SearchUsers([FromQuery] SearchUsersQuery query)
        => await HandleCommand(query);

    [HttpGet("history")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PaginatedList<UserHistory>))]
    public async Task<IActionResult> SearchHistory([FromQuery] SearchHistoryQuery query)
        => await HandleCommand(query);

    [HttpGet("{userId:guid}/history")]
    [Authorize(Roles = Constants.MIN_TEACHER_ROLE)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PaginatedList<UserHistory>))]
    public async Task<IActionResult> SearchHistoryByUserId([FromRoute] Guid userId, [FromQuery] SearchHistoryQuery query)
    {
        query.UserId = userId;
        return await HandleCommand(query);
    }

    [HttpGet("{userId:guid}")]
    [Authorize(Roles = Constants.MIN_TEACHER_ROLE)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(User))]
    public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
            => await HandleCommand(new GetUserByIdQuery(userId));

}
