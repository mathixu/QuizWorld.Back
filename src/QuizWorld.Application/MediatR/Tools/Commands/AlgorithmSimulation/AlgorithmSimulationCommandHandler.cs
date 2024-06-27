using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.Services;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Tools.Commands.AlgorithmSimulation;

public class AlgorithmSimulationCommandHandler(IQuizService quizService, IQuestionRepository questionRepository) : IRequestHandler<AlgorithmSimulationCommand, QuizWorldResponse<AlgorithmSimulationResponse>>
{
    private readonly IQuizService _quizService = quizService;
    private readonly IQuestionRepository _questionRepository = questionRepository;

    public async Task<QuizWorldResponse<AlgorithmSimulationResponse>> Handle(AlgorithmSimulationCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizService.GetByIdAsync(request.QuizId)
            ?? throw new NotFoundException(nameof(Quiz), request.QuizId);

        var questions = await _questionRepository.GetQuestionsByQuizIdAsync(request.QuizId, Status.Valid);

        List<UserResponse> userResponses = [];

        foreach(var question in questions)
        {
            if (new Random().NextDouble() > 0.3) 
                userResponses.Add(GenerateUserResponse(quiz, question, request.Turn));
        }

        var customQuestions = QuestionService.SortQuestions(quiz, userResponses, questions);

        // Transform customQuestions key to key.ToMinimal(). I need the dictionnary
        var customQuestionsMinimal = customQuestions
                                                                    .ToDictionary(x => x.Key.ToMinimal(), x => x.Value)
                                                                    .OrderByDescending(x => x.Value)
                                                                    .ToDictionary(x => x.Key, x => x.Value);

        var response = new AlgorithmSimulationResponse
        {
            QuizId = request.QuizId,
            Turn = request.Turn,
            UserResponses = userResponses.OrderBy(x => x.SuccessRate).ToList(),
            Questions = customQuestionsMinimal.ToList()
        };

        return QuizWorldResponse<AlgorithmSimulationResponse>.Success(response);
    }

    private static UserResponse GenerateUserResponse(Quiz quiz, Question question, int turn)
    {
        var userResponse = new UserResponse
        {
            QuizId = quiz.Id,
            User = new UserTiny
            {
                Id = Guid.Empty,
                FullName = "_"
            },
            SkillId = question.Skill.Id,
            Question = question.ToMinimal(),
            Attempts = new Random().Next(1, turn),
            SuccessRate = new Random().NextDouble(),
            LastResponseIsCorrect = new Random().NextDouble() > 0.5,
            UpdatedAt = DateTime.UtcNow.AddMinutes(new Random().Next(1, 200)),
            CreatedAt = DateTime.UtcNow
        };

        return userResponse;
    }
}
