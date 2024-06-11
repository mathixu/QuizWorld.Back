using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetSession;

public class GetSessionQueryHandler(ISessionService sessionService) : IRequestHandler<GetSessionQuery, QuizWorldResponse<Session>>
{
    private readonly ISessionService _sessionService = sessionService;

    public async Task<QuizWorldResponse<Session>> Handle(GetSessionQuery request, CancellationToken cancellationToken)
    {
        var response = await _sessionService.GetSessionByCode(request.Code)
            ?? throw new NotFoundException(nameof(Session), request.Code);
    
        return QuizWorldResponse<Session>.Success(response);
    }
}
