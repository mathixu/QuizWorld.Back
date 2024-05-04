using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Quizzes.Commands.CreateQuiz;

/// <summary>
/// Represents a command handler to create a quiz.
/// </summary>
public class CreateQuizCommandHandler(IQuizService quizService, IQuestionService questionService) : IRequestHandler<CreateQuizCommand, QuizWorldResponse<Quiz>>
{
    private readonly IQuizService _quizService = quizService;
    private readonly IQuestionService _questionService = questionService;

    public async Task<QuizWorldResponse<Quiz>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizService.CreateQuizAsync(request);

        // TODO: Call the command to create the questions if the quiz hasn't file
        if (!request.HasFile && !request.PersonalizedQuestions)
        {
            await _questionService.CreateQuestionsAsync(quiz);
        }

        return QuizWorldResponse<Quiz>.Success(quiz, 201);
    }
}