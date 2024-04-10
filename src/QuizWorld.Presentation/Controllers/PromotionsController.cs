using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Promotions.Commands.CreatePromotion;
using QuizWorld.Application.MediatR.Promotions.Commands.DeletePromotion;
using QuizWorld.Application.MediatR.Promotions.Queries.GetPromotions;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace QuizWorld.Presentation.Controllers;

/// <summary>
/// The promotions controller.
/// </summary>
[Authorize(Policy = AvailableRoles.Teacher)]
[SwaggerResponse(StatusCodes.Status401Unauthorized)]
public class PromotionsController : BaseApiController
{
    public PromotionsController(ISender sender) : base(sender)
    {
    }

    /// <summary>Creates a promotion.</summary>
    /// <param name="request">The DTO with the promotion's data.</param>
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(Promotion))]
    [HttpPost]
    public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionCommand request)
    {
        var response = await _sender.Send(request);

        return HandleResult(response);
    }

    /// <summary>Deletes a promotion.</summary>
    /// <param name="promotionId">The ID of the promotion to delete.</param>
    [SwaggerResponse(StatusCodes.Status404NotFound)]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    [HttpDelete("{promotionId:guid}")]
    public async Task<IActionResult> DeletePromotion([FromRoute] Guid promotionId)
    {
        var response = await _sender.Send(new DeletePromotionCommand(promotionId));

        return HandleResult(response);
    }

    /// <summary>Gets promotions.</summary>
    /// <param name="query">The query with the filter parameters</param>
    /// <returns>The promotions.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PaginatedList<PromotionTiny>))]
    [HttpGet]
    public async Task<IActionResult> GetPromotions([FromQuery] GetPromotionsQuery query)
    {
        var response = await _sender.Send(query);

        return HandleResult(response);
    }
}
