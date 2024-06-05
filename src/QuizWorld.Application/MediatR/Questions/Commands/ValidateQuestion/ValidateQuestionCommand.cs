using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;
using System.Text.Json.Serialization;

namespace QuizWorld.Application.MediatR.Questions.Commands.ValidateQuestion;

/// <summary>
/// Represents the validate question command.
/// </summary>
public class ValidateQuestionCommand : IQuizWorldRequest<Question>
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
    /// If the question is valid.
    /// </summary>
    public bool IsValid { get; set; }
}
