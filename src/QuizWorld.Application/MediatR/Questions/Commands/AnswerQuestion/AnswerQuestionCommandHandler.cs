using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Questions.Commands.AnswerQuestion;

public class AnswerQuestionCommandHandler(ISessionService sessionService, IQuestionService questionService) : IRequestHandler<AnswerQuestionCommand, QuizWorldResponse<Unit>>
{
    private readonly ISessionService _sessionService = sessionService;
    private readonly IQuestionService _questionService = questionService;

    public async Task<QuizWorldResponse<Unit>> Handle(AnswerQuestionCommand request, CancellationToken cancellationToken)
    {
        var currentUserSession = _sessionService.GetCurrentUserSession();

        if (currentUserSession.Status != UserSessionStatus.Connected)
            return QuizWorldResponse<Unit>.Failure("You are not connected to a session.");

        await _questionService.ProcessUserResponse(currentUserSession, request);

        return QuizWorldResponse<Unit>.Success(Unit.Value, 204);
    }
}
