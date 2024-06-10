using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestionStatus;

public class UpdateQuestionStatusCommandHandler(IQuestionService questionService) : IRequestHandler<UpdateQuestionStatusCommand, QuizWorldResponse<Question>>
{
    private readonly IQuestionService _questionService = questionService;

    public async Task<QuizWorldResponse<Question>> Handle(UpdateQuestionStatusCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionService.UpdateQuestionStatus(request.QuizId, request.QuestionId, request.Status);

        return QuizWorldResponse<Question>.Success(question, 200);
    }
}
