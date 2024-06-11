using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Quizzes.Commands.RegenerateQuestion
{
    public class RegenerateQuestionHandler(IQuestionService questionService) : IRequestHandler<RegenerateQuestionCommand, QuizWorldResponse<Question>>
    {
        private readonly IQuestionService _questionService = questionService;

        public async Task<QuizWorldResponse<Question>> Handle(RegenerateQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var question = await _questionService.RegenerateQuestion(request.QuizId, request.QuestionId, request.Requirement);

                return QuizWorldResponse<Question>.Success(question, 201);
            }
            catch (QuestionGenerationException)
            {
                return QuizWorldResponse<Question>.Failure("An unexpected error occurred during the generation of the question.", 500);
            }
        }
    }
}
