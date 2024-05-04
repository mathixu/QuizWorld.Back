using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Questions.Queries.GetQuestionsByQuizId;
using QuizWorld.Application.MediatR.Quizzes.Commands.AddAttachmentToQuiz;
using QuizWorld.Application.MediatR.Quizzes.Commands.CreateQuiz;
using QuizWorld.Application.MediatR.Quizzes.Queries.SearchQuizzes;
using QuizWorld.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace QuizWorld.Presentation.Controllers;

/// <summary>
/// Represents a controller for quizzes.
/// </summary>
public class QuizzesController(ISender sender) : BaseApiController(sender)
{
    /// <summary>Creates a new quiz.</summary>
    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(Quiz))]
    public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizCommand command)
        => await HandleCommand(command);

    /// <summary>Attaches a file to a quiz.</summary>
    [HttpPost("{quizId:guid}/attachment")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Quiz))]
    public async Task<IActionResult> AddAttachmentToQuiz([FromRoute] Guid quizId, [FromForm] IFormFileCollection attachment) // TODO: Change to IFormFile when Swagger supports it
        => await HandleCommand(new AddAttachmentToQuizCommand(quizId, attachment.First())); 

    /// <summary>Search for quizzes by their name.</summary>
    [HttpGet]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PaginatedList<QuizTiny>))]
    public async Task<IActionResult> SearchQuizzes([FromQuery] SearchQuizzesQuery query)
        => await HandleCommand(query);

    [HttpGet("{quizId:guid}/questions")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PaginatedList<Question>))]
    public async Task<IActionResult> GetQuestionsByQuizId([FromRoute] Guid quizId, [FromQuery] int page = 1, [FromQuery] int pageSize = 25)
        => await HandleCommand(new GetQuestionsByQuizIdQuery(quizId, page, pageSize));
}