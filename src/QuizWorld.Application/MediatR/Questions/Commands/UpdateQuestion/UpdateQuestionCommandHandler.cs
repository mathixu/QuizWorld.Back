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
    public class UpdateQuestionCommandHandler(ISessionService sessionService, IQuestionService questionService) : IRequestHandler<UpdateQuestionCommand, QuizWorldResponse<QuestionTiny>>
    {
        private readonly ISessionService _sessionService = sessionService;
        private readonly IQuestionService _questionService = questionService;

        public async Task<QuizWorldResponse<QuestionTiny>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var currentUserSession = _sessionService.GetCurrentUserSession();

            if (currentUserSession.Status != UserSessionStatus.Connected)
                return QuizWorldResponse<QuestionTiny>.Failure("You are not connected to a session.");

            return QuizWorldResponse<QuestionTiny>.Success(QuestionTiny);
        }
    }
}
