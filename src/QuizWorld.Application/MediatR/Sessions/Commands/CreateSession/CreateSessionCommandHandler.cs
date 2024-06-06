using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Sessions.Commands.CreateSession;

public class CreateSessionCommandHandler(ISessionService sessionService) : IRequestHandler<CreateSessionCommand, QuizWorldResponse<Session>>
{
    private readonly ISessionService _sessionService = sessionService;

    public async Task<QuizWorldResponse<Session>> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionService.CreateSession(request.QuizId);

        return QuizWorldResponse<Session>.Success(session, 201);
    }
}
