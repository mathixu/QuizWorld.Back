using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

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

        if (!request.HasFile)
        {
            try
            {
                await _questionService.CreateQuestionsAsync(quiz);

                await _quizService.UpdateQuizStatus(quiz.Id, QuizStatus.Pending);

                quiz.Status = QuizStatus.Pending;
            } 
            catch(QuestionGenerationException)
            {
                await _quizService.UpdateQuizStatus(quiz.Id, QuizStatus.Inactive);

                quiz.Status = QuizStatus.Inactive;
            }
        }

        return QuizWorldResponse<Quiz>.Success(quiz, 201);
    }
}