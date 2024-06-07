using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;
using System.Text.Json.Serialization;

namespace QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestion
{
    /// <summary>
    /// Represents a command to update a question.
    /// </summary>
    public class UpdateQuestionCommand : IQuizWorldRequest<Question>
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
        /// The generated question to update.
        /// </summary>
        public GeneratedQuestion Question { get; set; } = default!;

    }
}
