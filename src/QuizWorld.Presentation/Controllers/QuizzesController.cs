using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Questions.Commands.AnswerQuestion;
using QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestion;
using QuizWorld.Application.MediatR.Questions.Queries.GetQuestionsByQuizId;
using QuizWorld.Application.MediatR.Quizzes.Commands.AddAttachmentToQuiz;
using QuizWorld.Application.MediatR.Quizzes.Commands.CreateQuiz;
using QuizWorld.Application.MediatR.Quizzes.Commands.StartQuiz;
using QuizWorld.Application.MediatR.Quizzes.Queries.SearchQuizzes;
using QuizWorld.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using QuizWorld.Application.MediatR.Quizzes.Queries.GetQuizById;
using QuizWorld.Application.MediatR.Quizzes.Commands.ValidateQuiz;
using QuizWorld.Application.MediatR.Quizzes.Commands.RegenerateQuestion;
using QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestionStatus;
using QuizWorld.Presentation.WebSockets;

namespace QuizWorld.Presentation.Controllers;

/// <summary>
/// Represents a controller for quizzes.
/// </summary>
[Authorize(Roles = Constants.MIN_TEACHER_ROLE)]
public class QuizzesController(ISender sender, WebSocketService webSocketService) : BaseApiController(sender)
{
    private readonly WebSocketService _webSocketService = webSocketService;

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

    /// <summary>Gets the questions of a quiz.</summary>
    [HttpGet("{quizId:guid}/questions")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PaginatedList<Question>))]
    public async Task<IActionResult> GetQuestionsByQuizId([FromRoute] Guid quizId, [FromQuery] PaginationQuery query)
        => await HandleCommand(new GetQuestionsByQuizIdQuery(quizId, query.Page, query.PageSize));

    /// <summary>Starts a quiz.</summary>
    [HttpPost("{quizId:guid}/start")]
    [Authorize(Roles = Constants.MIN_STUDENT_ROLE)]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(StartQuizResponse))]
    public async Task<IActionResult> StartQuiz([FromRoute] Guid quizId)
    {
        var result = await _sender.Send(new StartQuizCommand(quizId));

        if (result.IsSuccessful)
        {
            await _webSocketService.HandleAction(WebSocketAction.UserStartedQuiz);
        }

        return HandleResult(result);
    }

    /// <summary>Answers a question.</summary>
    [HttpPost("{quizId:guid}/questions/{questionId:guid}/answer")]
    [Authorize(Roles = Constants.MIN_STUDENT_ROLE)]
    public async Task<IActionResult> AnswerQuestion([FromRoute] Guid quizId, [FromRoute] Guid questionId, [FromBody] AnswerQuestionCommand command)
    {
        command.QuizId = quizId;
        command.QuestionId = questionId;

        var result = await _sender.Send(command);

        if (result.IsSuccessful)
        {
            await _webSocketService.HandleAction(result.Data.Action);
        }

        return HandleResult(result);
    }

    [HttpPut("{quizId:guid}/questions/{questionId:guid}/status")]
    public async Task<IActionResult> ChangeQuestionStatus([FromRoute] Guid quizId, [FromRoute] Guid questionId, [FromBody] UpdateQuestionStatusCommand command)
    {
        command.QuizId = quizId;
        command.QuestionId = questionId;

        return await HandleCommand(command);
    }

    /// <summary>Edit a question or answer.</summary>
    [HttpPut("{quizId:guid}/questions/{questionId:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Question))]
    public async Task<IActionResult> UpdateQuestion([FromRoute] Guid quizId, [FromRoute] Guid questionId, [FromBody] UpdateQuestionCommand command)
    {
        command.QuizId = quizId;
        command.QuestionId = questionId;
        return await HandleCommand(command);
    }

    /// <summary>Regenerate a question.</summary>
    [HttpPut("{quizId:guid}/questions/{questionId:guid}/regenerate")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Question))]
    public async Task<IActionResult> RegenerateQuestion([FromRoute] Guid quizId, [FromRoute] Guid questionId, [FromBody] RegenerateQuestionCommand command)
    {
        command.QuizId = quizId;
        command.QuestionId = questionId;
        return await HandleCommand(command);
    }

    /// <summary>Validate a quiz.</summary>
    [HttpPost("validate")]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(Quiz))]
    public async Task<IActionResult> ValidateQuiz([FromBody] ValidateQuizCommand command)
        => await HandleCommand(command);

    [HttpGet("{quizId:guid}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Quiz))]
    public async Task<IActionResult> GetQuizById([FromRoute] Guid quizId)
        => await HandleCommand(new GetQuizByIdQuery(quizId));
}