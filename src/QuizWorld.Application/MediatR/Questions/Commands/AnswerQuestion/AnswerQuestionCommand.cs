using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using System.Text.Json.Serialization;

namespace QuizWorld.Application.MediatR.Questions.Commands.AnswerQuestion;

public class AnswerQuestionCommand : IQuizWorldRequest<AnswerQuestionResponse>
{
    /// <summary>
    /// Represents the id of the quiz.
    /// </summary>
    [JsonIgnore]
    public Guid QuizId { get; set; } = default!;

    /// <summary>
    /// Represents the id of the question.
    /// </summary>
    [JsonIgnore]
    public Guid QuestionId { get; set; } = default!;

    /// <summary>
    /// Represents the answer ids.
    /// </summary>
    public List<Guid> AnswerIds { get; set; } = [];
}

public record AnswerQuestionResponse(Guid SessionId, WebSocketAction Action);