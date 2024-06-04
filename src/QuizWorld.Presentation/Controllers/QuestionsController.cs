using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestion;
using QuizWorld.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace QuizWorld.Presentation.Controllers;

    [Authorize(Roles = Constants.MIN_TEACHER_ROLE)]
    public class QuestionsController(ISender sender) : BaseApiController(sender)
    {
        [HttpPut]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(QuestionTiny))]
        public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionCommand command)
            => await HandleCommand(command);
    }