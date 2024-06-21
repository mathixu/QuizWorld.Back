using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Enums;
using System.Collections.Concurrent;

namespace QuizWorld.Application.MediatR.Questions.Commands.AnswerQuestion;

public class AnswerQuestionCommandHandler(ISessionService sessionService, IQuestionService questionService, ICurrentUserService currentUserService) : IRequestHandler<AnswerQuestionCommand, QuizWorldResponse<Unit>>
{
    private readonly ISessionService _sessionService = sessionService;
    private readonly IQuestionService _questionService = questionService;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> _userResponseLock = new ConcurrentDictionary<Guid, SemaphoreSlim>();

    public async Task<QuizWorldResponse<Unit>> Handle(AnswerQuestionCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId 
            ?? throw new UnauthorizedAccessException("You're not authenticated.");

        var semaphore = GetSemaphoreForUser(currentUserId);

        await semaphore.WaitAsync(cancellationToken);

        try
        {
            var currentUserSession = await _sessionService.GetCurrentUserSession();

            if (currentUserSession.Status != UserSessionStatus.Connected)
                return QuizWorldResponse<Unit>.Failure("You are not connected to a session.");

            await _questionService.ProcessUserResponse(currentUserSession, request);

            return QuizWorldResponse<Unit>.Success(Unit.Value, 204);
        }
        finally
        {
            semaphore.Release();
        }
    }

    private static SemaphoreSlim GetSemaphoreForUser(Guid userId)
    {
        return _userResponseLock.GetOrAdd(userId, _ => new SemaphoreSlim(1, 1));
    }
}
