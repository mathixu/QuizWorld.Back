
using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetSessionById;

public class GetSessionByIdQueryHandler(ISessionRepository sessionRepository) : IRequestHandler<GetSessionByIdQuery, QuizWorldResponse<Session>>
{
    private readonly ISessionRepository _sessionRepository = sessionRepository;

    public async Task<QuizWorldResponse<Session>> Handle(GetSessionByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _sessionRepository.GetByIdAsync(request.SessionId)
            ?? throw new NotFoundException(nameof(Session), request.SessionId);

        return QuizWorldResponse<Session>.Success(response);
    }
}
