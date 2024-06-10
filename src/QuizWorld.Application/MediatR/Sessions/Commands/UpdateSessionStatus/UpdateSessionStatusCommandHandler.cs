using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Sessions.Commands.UpdateSessionStatus;

public class UpdateSessionStatusCommandHandler(ISessionService sessionService) : IRequestHandler<UpdateSessionStatusCommand, QuizWorldResponse<SessionTiny>>
{
    private readonly ISessionService _sessionService = sessionService;

    public async Task<QuizWorldResponse<SessionTiny>> Handle(UpdateSessionStatusCommand request, CancellationToken cancellationToken)
    {
        var response = await _sessionService.UpdateSessionStatus(request.Code!, request.Status);

        return QuizWorldResponse<SessionTiny>.Success(response.ToTiny(), 200);
    }
}
