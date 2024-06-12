using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;
using System.Text.Json.Serialization;

namespace QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestionStatus;

/// <summary>
/// Represents the validate question command.
/// </summary>
public class UpdateQuestionStatusCommand : IQuizWorldRequest<Question>
{
    /// <summary>
    /// The quiz of the question.
    /// </summary>
    [JsonIgnore]
    public Guid QuizId { get; set; }

    /// <summary>
    /// The question to validate.
    /// </summary>
    [JsonIgnore]
    public Guid QuestionId { get; set; }

    /// <summary>
    /// The status of the question.
    /// </summary>
    public Status Status { get; set; }
}
