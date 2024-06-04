using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Questions.Commands.ValidateQuestion;

public class ValidateQuestionCommandHandler(IQuestionService questionService) : IRequestHandler<ValidateQuestionCommand, QuizWorldResponse<Question>>
{
    private readonly IQuestionService _questionService = questionService;

    public async Task<QuizWorldResponse<Question>> Handle(ValidateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionService.ValidateQuestion(request.QuizId, request.QuestionId, request.IsValid);

        return QuizWorldResponse<Question>.Success(question, 200);
    }
}
