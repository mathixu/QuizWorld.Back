using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Quizzes.Queries.SearchQuizzes;

public class SearchQuizzesQueryHandler(IQuizService quizService) : IRequestHandler<SearchQuizzesQuery, QuizWorldResponse<PaginatedList<QuizTiny>>>
{
    private readonly IQuizService _quizService = quizService;

    public async Task<QuizWorldResponse<PaginatedList<QuizTiny>>> Handle(SearchQuizzesQuery request, CancellationToken cancellationToken)
    {
        var quizzes = await _quizService.SearchQuizzesAsync(request);

        return QuizWorldResponse<PaginatedList<QuizTiny>>.Success(quizzes, 200);
    }
}
