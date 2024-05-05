using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace QuizWorld.Presentation.Controllers;

/// <summary>
/// Represents the base API controller used by all controllers.
/// </summary>
[ApiController]
[Route("[controller]")]
[SwaggerResponse(StatusCodes.Status400BadRequest)]
[RequiredScopeOrAppPermission(RequiredScopesConfigurationKey = Constants.KEY_VAULT_SECRET_AZURE_AD_SCOPES)] // useless ?
[Authorize(Roles = Constants.MIN_STUDENT_ROLE)]
public abstract class BaseApiController(ISender sender) : ControllerBase
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
            return StatusCode(response.StatusCode, response.StatusCode != 204 ? response.Data : null);

        return StatusCode(response.StatusCode, new { message = response.ErrorMessage });
    }

    /// <summary>
    /// Handles the command request to return the appropriate response.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request to handle.</param>
    /// <returns>The appropriate response.</returns>
    protected async Task<IActionResult> HandleCommand<TResponse>(IQuizWorldRequest<TResponse> request)
    {
        var response = await _sender.Send(request);
        
        return HandleResult(response);
    }
}
