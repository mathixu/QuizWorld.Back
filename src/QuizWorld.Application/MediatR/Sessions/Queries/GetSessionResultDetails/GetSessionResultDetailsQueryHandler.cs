using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetQuizResultDetails;

public class GetSessionResultDetailsQueryHandler(ISessionService sessionService) : IRequestHandler<GetSessionResultDetailsQuery, QuizWorldResponse<List<UserAnswer>>>
{
    private readonly ISessionService _sessionService = sessionService;

    public async Task<QuizWorldResponse<List<UserAnswer>>> Handle(GetSessionResultDetailsQuery request, CancellationToken cancellationToken)
    {
        var userAnswers = await _sessionService.GetUserQuizResult(request.SessionId, request.UserId);

        return QuizWorldResponse<List<UserAnswer>>.Success(userAnswers);
    }
}
