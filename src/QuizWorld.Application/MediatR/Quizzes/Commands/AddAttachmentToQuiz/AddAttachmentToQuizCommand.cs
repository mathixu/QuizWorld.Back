using Microsoft.AspNetCore.Http;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Quizzes.Commands.AddAttachmentToQuiz;

/// <summary>
/// Represents a command to add an attachment to a quiz.
/// </summary>
/// <param name="QuizId">The identifier of the quiz to add the attachment to.</param>
/// <param name="Attachment">The attachment to add to the quiz.</param>
public record AddAttachmentToQuizCommand(Guid QuizId, IFormFile Attachment) : IQuizWorldRequest<Quiz>;