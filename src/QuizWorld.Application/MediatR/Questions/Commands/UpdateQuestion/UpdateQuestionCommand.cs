using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;
using System.Text.Json.Serialization;

namespace QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestion
{
    public class UpdateQuestionCommand : IQuizWorldRequest<QuestionTiny>
    {
        [JsonIgnore]
        public Guid QuizId { get; set; } = default!;

        [JsonIgnore]
        public Guid QuestionId { get; set; } = default!;

        public string QuestionName { get; set; } = default!;

        public List<Guid> AnswerIds { get; set; } = [];
    }
}
