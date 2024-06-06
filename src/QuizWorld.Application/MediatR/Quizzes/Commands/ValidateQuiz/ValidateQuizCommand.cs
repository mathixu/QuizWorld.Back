using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Quizzes.Commands.ValidateQuiz
{
    public class ValidateQuizCommand : IQuizWorldRequest<Quiz>
    {
        /// <summary>
        /// Represents the quiz id of the session.
        /// </summary>
        public Guid QuizId { get; set; } = default!;
    }
}
