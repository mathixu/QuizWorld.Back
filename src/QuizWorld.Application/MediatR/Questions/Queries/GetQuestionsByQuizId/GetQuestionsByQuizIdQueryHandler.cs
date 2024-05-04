using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Questions.Queries.GetQuestionsByQuizId;

public class GetQuestionsByQuizIdQueryHandler(IQuestionRepository questionRepository) : IRequestHandler<GetQuestionsByQuizIdQuery, QuizWorldResponse<PaginatedList<Question>>>
{
    private readonly IQuestionRepository _questionRepository = questionRepository;

    public async Task<QuizWorldResponse<PaginatedList<Question>>> Handle(GetQuestionsByQuizIdQuery request, CancellationToken cancellationToken)
    {
        var questions = await _questionRepository.GetQuestionsByQuizIdAsync(request.QuizId, request.Page, request.PageSize);

        return QuizWorldResponse<PaginatedList<Question>>.Success(questions, 200);
    }
}
