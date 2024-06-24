using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetSessionHistory;

public class GetSessionHistoryQueryHandler(ICurrentUserService currentUserService, ISessionService sessionService) : IRequestHandler<GetSessionHistoryQuery, QuizWorldResponse<PaginatedList<SessionLight>>>
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly ISessionService _sessionService = sessionService;

    public async Task<QuizWorldResponse<PaginatedList<SessionLight>>> Handle(GetSessionHistoryQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId 
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var sessions = await _sessionService.GetSessionHistory(userId, request);

        return QuizWorldResponse<PaginatedList<SessionLight>>.Success(sessions.Map(s => s.ToLight()), 200);
    }
}
