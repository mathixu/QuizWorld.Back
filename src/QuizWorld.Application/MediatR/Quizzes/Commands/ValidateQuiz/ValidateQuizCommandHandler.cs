using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Quizzes.Commands.ValidateQuiz
{
    public class ValidateQuizCommandHandler(IQuizService quizService) : IRequestHandler<ValidateQuizCommand, QuizWorldResponse<Quiz>>
    {
        private readonly IQuizService _quizService = quizService;
        public async Task<QuizWorldResponse<Quiz>> Handle(ValidateQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _quizService.ValidateQuizAsync(request.QuizId);

            return QuizWorldResponse<Quiz>.Success(quiz, 200);
        }
    }
}
