using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Questions.Commands.AnswerQuestion;

public class AnswerQuestionCommandHandler(ISessionService sessionService, IQuestionService questionService, IUserSessionRepository userSessionRepository, IUserResponseRepository userResponseRepository) : IRequestHandler<AnswerQuestionCommand, QuizWorldResponse<Unit>>
{
    private readonly ISessionService _sessionService = sessionService;
    private readonly IQuestionService _questionService = questionService;
    private readonly IUserSessionRepository _userSessionRepository = userSessionRepository;
    private readonly IUserResponseRepository _userResponseRepository = userResponseRepository;

    public async Task<QuizWorldResponse<Unit>> Handle(AnswerQuestionCommand request, CancellationToken cancellationToken)
    {
        var currentUserSession = _sessionService.GetCurrentUserSession();

        if (currentUserSession.Status != UserSessionStatus.Connected)
            return QuizWorldResponse<Unit>.Failure("You are not connected to a session.");

        var question = await _questionService.GetQuestionById(request.QuestionId)
            ?? throw new NotFoundException(nameof(Question), request.QuestionId);

        if (question.QuizId != request.QuizId)
            return QuizWorldResponse<Unit>.Failure("The question does not belong to the quiz.");

        if (!request.AnswerIds.All(x => question.HasAnswer(x)))
            return QuizWorldResponse<Unit>.Failure("One or more answers do not belong to the question.");

        var userResponse = await _userResponseRepository.GetUserResponseByUserSessionId(currentUserSession.Id);

        var response = new Responses
        {
            Answers = question.ExtractAnswers(request.AnswerIds),
            IsCorrect = question.CheckAnswer(request.AnswerIds),
            Question = question.ToMinimal(),
            QuizId = request.QuizId,
        };

        if (userResponse is not null)
        {
            await _userResponseRepository.AddResponseAsync(userResponse.Id, response);
        } 
        else
        {
            var userResponseBuilded = new UserResponse
            {
                UserSessionId = currentUserSession.Id,
                User = currentUserSession.User,
                Responses = [response]
            };

            await _userResponseRepository.AddAsync(userResponseBuilded);
        }

        return QuizWorldResponse<Unit>.Success(Unit.Value, 204);
    }
}
