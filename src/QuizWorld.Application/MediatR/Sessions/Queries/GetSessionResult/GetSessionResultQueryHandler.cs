using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetSessionResult;

public class GetSessionResultQueryHandler(ISessionService sessionService) : IRequestHandler<GetSessionResultQuery, QuizWorldResponse<UserSessionResult>>
{
    private readonly ISessionService _sessionService = sessionService;

    public async Task<QuizWorldResponse<UserSessionResult>> Handle(GetSessionResultQuery request, CancellationToken cancellationToken)
    {
        var response = await _sessionService.GetSessionResult(request.Code)
            ?? throw new BadRequestException("Session not found.");

        return QuizWorldResponse<UserSessionResult>.Success(response);
    }
}
