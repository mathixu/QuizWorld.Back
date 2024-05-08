using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.MediatR.Users.Queries.GetCurrentUser;

namespace QuizWorld.Presentation.Controllers;

public class UsersController(ISender sender) : BaseApiController(sender)
{
    [HttpGet("~/profile")]
    public async Task<IActionResult> GetCurrentUser()
        => await HandleCommand(new GetCurrentUserQuery());
}
