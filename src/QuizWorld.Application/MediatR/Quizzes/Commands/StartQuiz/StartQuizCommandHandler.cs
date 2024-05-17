using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;

namespace QuizWorld.Application.MediatR.Quizzes.Commands.StartQuiz;

public class StartQuizCommandHandler(ISessionService sessionService) : IRequestHandler<StartQuizCommand, QuizWorldResponse<StartQuizResponse>>
{
    private readonly ISessionService _sessionService = sessionService;

    public async Task<QuizWorldResponse<StartQuizResponse>> Handle(StartQuizCommand request, CancellationToken cancellationToken)
    {
        var response = await _sessionService.StartQuiz(request.QuizId);

        return QuizWorldResponse<StartQuizResponse>.Success(response);
    }
}
