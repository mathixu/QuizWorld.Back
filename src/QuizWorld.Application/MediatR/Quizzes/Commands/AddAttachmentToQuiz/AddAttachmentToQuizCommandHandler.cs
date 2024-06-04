using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Quizzes.Commands.AddAttachmentToQuiz;

/// <summary>
/// Represents a command handler to add an attachment to a quiz.
/// </summary>
public class AddAttachmentToQuizCommandHandler(IQuizService quizService, IQuestionService questionService) : IRequestHandler<AddAttachmentToQuizCommand, QuizWorldResponse<Quiz>>
{
    private readonly IQuizService _quizService = quizService;
    private readonly IQuestionService _questionService = questionService;

    public async Task<QuizWorldResponse<Quiz>> Handle(AddAttachmentToQuizCommand request, CancellationToken cancellationToken)
    {
        var success = await _quizService.AddAttachmentToQuiz(request.QuizId, request.Attachment);

        if (!success)
            return QuizWorldResponse<Quiz>.Failure("Failed to add an attachment", 400);

        var quiz = await _quizService.GetByIdAsync(request.QuizId)
            ?? throw new NotFoundException(nameof(Quiz), request.QuizId);

        try
        {
            await _questionService.CreateQuestionsAsync(quiz);

            await _quizService.UpdateQuizStatus(quiz.Id, QuizStatus.Pending);

            quiz.Status = QuizStatus.Pending;
        }
        catch (QuestionGenerationException)
        {
            await _quizService.UpdateQuizStatus(quiz.Id, QuizStatus.Inactive);

            quiz.Status = QuizStatus.Inactive;
        }

        return QuizWorldResponse<Quiz>.Success(quiz);
    }
}
