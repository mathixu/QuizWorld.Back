using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Questions.Commands.AnswerQuestion;

public class AnswerQuestionCommandHandler(ISessionService sessionService, IQuestionService questionService) : IRequestHandler<AnswerQuestionCommand, QuizWorldResponse<AnswerQuestionResponse>>
{
    private readonly ISessionService _sessionService = sessionService;
    private readonly IQuestionService _questionService = questionService;

    public async Task<QuizWorldResponse<AnswerQuestionResponse>> Handle(AnswerQuestionCommand request, CancellationToken cancellationToken)
    {
        var currentUserSession = await _sessionService.GetCurrentUserSession();

        if (currentUserSession.Status != UserSessionStatus.Connected)
            return QuizWorldResponse<AnswerQuestionResponse>.Failure("You are not connected to a session.");

        var action = await _questionService.ProcessUserResponse(currentUserSession, request);

        var response = new AnswerQuestionResponse(currentUserSession.Session.Id, action);

        return QuizWorldResponse<AnswerQuestionResponse>.Success(response, 204);
    }
}
