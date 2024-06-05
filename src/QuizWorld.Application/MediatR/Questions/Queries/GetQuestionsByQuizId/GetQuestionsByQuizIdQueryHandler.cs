using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Questions.Queries.GetQuestionsByQuizId;

public class GetQuestionsByQuizIdQueryHandler(IQuestionRepository questionRepository) : IRequestHandler<GetQuestionsByQuizIdQuery, QuizWorldResponse<List<Question>>>
{
    private readonly IQuestionRepository _questionRepository = questionRepository;

    public async Task<QuizWorldResponse<List<Question>>> Handle(GetQuestionsByQuizIdQuery request, CancellationToken cancellationToken)
    {
        var questions = await _questionRepository.GetQuestionsByQuizIdAsync(request.QuizId);

        return QuizWorldResponse<List<Question>>.Success(questions, 200);
    }
}
