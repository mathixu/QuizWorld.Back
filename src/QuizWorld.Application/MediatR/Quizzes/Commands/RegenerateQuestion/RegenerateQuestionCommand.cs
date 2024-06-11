using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;
using System.Text.Json.Serialization;

namespace QuizWorld.Application.MediatR.Quizzes.Commands.RegenerateQuestion
{
    /// <summary>
    /// Represent a command to regenerate a question
    /// </summary>
    public class RegenerateQuestionCommand : IQuizWorldRequest<Question>
    {
        /// <summary>
        /// The id of a Quiz.
        /// </summary>
        [JsonIgnore]
        public Guid QuizId { get; set; } = default!;

        /// <summary>
        /// The id of a Question.
        /// </summary>
        [JsonIgnore]
        public Guid QuestionId { get; set; } = default!;

        /// <summary>
        /// The requirement for the question.
        /// </summary>
        public string Requirement { get; set; } = default!;

        /// <summary>If the quiz has a file.</summary>
        public bool HasFile { get; set; }
    }
}
