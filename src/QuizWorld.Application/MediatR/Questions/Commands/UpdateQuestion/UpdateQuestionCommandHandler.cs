using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Questions.Commands.AnswerQuestion;
using QuizWorld.Application.Services;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestion
{
    public class UpdateQuestionCommandHandler(IQuestionService questionService) : IRequestHandler<UpdateQuestionCommand, QuizWorldResponse<bool>>
    {
        private readonly IQuestionService _questionService = questionService;

        public async Task<QuizWorldResponse<bool>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            await _questionService.UpdateQuestion(request);
            return QuizWorldResponse<bool>.Success(true);
        }
    }
}
