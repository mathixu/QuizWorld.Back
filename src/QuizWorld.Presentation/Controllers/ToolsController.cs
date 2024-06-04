using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace QuizWorld.Presentation.Controllers;

[AllowAnonymous]
public class ToolsController(ISender sender, IQuestionGenerator questionGenerator) : BaseApiController(sender)
{
    private readonly IQuestionGenerator _questionGenerator = questionGenerator;

    [HttpPost("format-generated-content")]
    public async Task<IActionResult> FormatGeneratedContent([FromBody] string content)
    {
        return Ok(JsonSerializer.Deserialize(content.FormatFromLLM(), typeof(object)));
    }

    [HttpPost("generate-questions")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<Question>))]
    public async Task<IActionResult> GenerateQuestions([FromBody] GenerateQuestionCommand command)
    {
        try
        {
            var skillTiny = new SkillTiny
            {
                Name = command.Skill
            };

            var questions = await _questionGenerator.GenerateQuestionsBySkills(Guid.Empty, skillTiny, command.TotalQuestions, 1);

            return Ok(questions);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class GenerateQuestionCommand 
{
    public string Skill { get; set; }
    public int TotalQuestions { get; set; }
}
