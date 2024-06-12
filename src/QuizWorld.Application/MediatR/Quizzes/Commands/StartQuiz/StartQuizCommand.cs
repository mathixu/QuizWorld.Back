using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Quizzes.Commands.StartQuiz;

public record StartQuizCommand(Guid QuizId) : IQuizWorldRequest<StartQuizResponse>;

/// <summary>
/// Represents the response of the start quiz command.
/// </summary>
public class StartQuizResponse
{
    /// <summary>
    /// Represents the title of the quiz.
    /// </summary>
    public List<QuestionTiny> Questions { get; set; } = [];

    /// <summary>
    /// Represents the attachment of the quiz.
    /// </summary>
    public QuizFile? Attachment { get; set; } = default!;
}