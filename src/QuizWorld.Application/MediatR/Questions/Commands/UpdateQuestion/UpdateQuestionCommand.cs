using MongoDB.Bson.Serialization.Attributes;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;
using System.Text.Json.Serialization;

namespace QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestion
{
    public class UpdateQuestionCommand : IQuizWorldRequest<bool>
    {
        [JsonIgnore]
        public Guid QuizId { get; set; } = default!;

        [JsonIgnore]
        public Guid QuestionId { get; set; } = default!;

        public GeneratedQuestion Question { get; set; } = default!;

    }
}
