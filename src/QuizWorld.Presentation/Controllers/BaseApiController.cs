using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.Common.Models;

namespace QuizWorld.Presentation.Controllers;

/// <summary>
/// Represents the base API controller used by all controllers.
/// </summary>
/// <param name="sender"></param>
[ApiController]
[Route("[controller]")]
public class BaseApiController(ISender sender) : ControllerBase
{
    /// <summary>
    /// The sender used to send requests with MediatR.
    /// </summary>
    protected readonly ISender _sender = sender;

    /// <summary>
    /// Handles the result of the request to return the appropriate response.
    /// </summary>
    /// <param name="response">The response to handle.</param>
    /// <returns>The appropriate response.</returns>
    protected IActionResult HandleResult<TResponse>(QuizWorldResponse<TResponse> response)
    {
        if (response.IsSuccessful)
            return StatusCode(response.StatusCode, response.Data);

        return StatusCode(response.StatusCode, new { message = response.ErrorMessage });
    }
}
