using MediatR;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Quizzes.Commands.CreateQuiz;

/// <summary>
/// Represents a command handler to create a quiz.
/// </summary>
public class CreateQuizCommandHandler(IQuizService quizService) : IRequestHandler<CreateQuizCommand, QuizWorldResponse<Quiz>>
{
    private readonly IQuizService _quizService = quizService;

    public async Task<QuizWorldResponse<Quiz>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizService.CreateQuizAsync(request);

        // TODO: Call the command to create the questions if the quiz hasn't file

        return QuizWorldResponse<Quiz>.Success(quiz, 201);
    }
}