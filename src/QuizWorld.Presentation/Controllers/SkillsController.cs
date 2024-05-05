
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.MediatR.Skills.Commands.CreateSkill;
using QuizWorld.Application.MediatR.Skills.Queries.SearchSkills;
using QuizWorld.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace QuizWorld.Presentation.Controllers;

public class SkillsController(ISender sender) : BaseApiController(sender)
{
    /// <summary>Creates a new skill.</summary>
    [HttpPost]
    [Authorize(Roles = Constants.MIN_TEACHER_ROLE)]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(Skill))]
    public async Task<IActionResult> CreateSkill([FromBody] CreateSkillCommand command)
        => await HandleCommand(command);

    /// <summary>Search for skills by their name.</summary>
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PaginatedList<SkillTiny>))]
    public async Task<IActionResult> SearchSkills([FromQuery] SearchSkillsQuery query)
        => await HandleCommand(query);
}
