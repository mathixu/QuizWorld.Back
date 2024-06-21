using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Quizzes.Queries.GetQuizById;

public class GetQuizByIdQueryHandler(IQuizService quizService) : IRequestHandler<GetQuizByIdQuery, QuizWorldResponse<Quiz>>
{
    private readonly IQuizService _quizService = quizService;

    public async Task<QuizWorldResponse<Quiz>> Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
    {
        var quiz = await _quizService.GetByIdAsync(request.QuizId)
            ?? throw new NotFoundException(nameof(Quiz), request.QuizId);

        return QuizWorldResponse<Quiz>.Success(quiz);
    }
}
