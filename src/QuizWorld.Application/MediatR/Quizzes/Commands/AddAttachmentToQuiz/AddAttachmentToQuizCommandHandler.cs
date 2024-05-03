using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Quizzes.Commands.AddAttachmentToQuiz;

/// <summary>
/// Represents a command handler to add an attachment to a quiz.
/// </summary>
public class AddAttachmentToQuizCommandHandler(IQuizService quizService) : IRequestHandler<AddAttachmentToQuizCommand, QuizWorldResponse<Quiz>>
{
    private readonly IQuizService _quizService = quizService;

    public async Task<QuizWorldResponse<Quiz>> Handle(AddAttachmentToQuizCommand request, CancellationToken cancellationToken)
    {
        var success = await _quizService.AddAttachmentToQuiz(request.QuizId, request.Attachment);

        if (!success)
            return QuizWorldResponse<Quiz>.Failure("Failed to add an attachment", 400);

        var quiz = await _quizService.GetByIdAsync(request.QuizId);

        return QuizWorldResponse<Quiz>.Success(quiz!);
    }
}
