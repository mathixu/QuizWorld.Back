using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Sessions.Commands.UpdateSessionStatus;

public class UpdateSessionStatusCommandHandler(ISessionService sessionService) : IRequestHandler<UpdateSessionStatusCommand, QuizWorldResponse<Session>>
{
    private readonly ISessionService _sessionService = sessionService;

    public async Task<QuizWorldResponse<Session>> Handle(UpdateSessionStatusCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionService.UpdateSessionStatus(request.Code!, request.Status);

        return QuizWorldResponse<Session>.Success(session, 200);
    }
}
