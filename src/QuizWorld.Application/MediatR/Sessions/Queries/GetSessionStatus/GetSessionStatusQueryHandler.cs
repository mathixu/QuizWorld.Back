using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetSessionStatus;

public class GetSessionStatusQueryHandler(ISessionService sessionService) : IRequestHandler<GetSessionStatusQuery, QuizWorldResponse<SessionStatusResponse>>
{
    private readonly ISessionService _sessionService = sessionService;

    public async Task<QuizWorldResponse<SessionStatusResponse>> Handle(GetSessionStatusQuery request, CancellationToken cancellationToken)
    {
        var response = await _sessionService.GetSessionStatus(request.Code);
    
        return QuizWorldResponse<SessionStatusResponse>.Success(response);
    }
}
