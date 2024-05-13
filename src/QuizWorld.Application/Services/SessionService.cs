using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Sessions.Queries.GetSessionStatus;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Services;

public class SessionService(IQuizService quizService, 
    ICurrentUserService currentUserService, 
    ISessionRepository sessionRepository) : ISessionService
{
    private readonly ISessionRepository _sessionRepository = sessionRepository;
    private readonly IQuizService _quizService = quizService;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    /// <inheritdoc />
    public async Task<Session> CreateSession(List<Guid> quizIds)
    {
        var currentUser = _currentUserService.UserTiny 
            ?? throw new UnauthorizedAccessException();

        var quizzes = await BuildQuizzes(quizIds);

        var session = new Session
        {
            Quizzes = quizzes,
            Code = GenerateCode(),
            CreatedBy = currentUser,
            Status = SessionStatus.Awaiting
        };

        await _sessionRepository.AddAsync(session);

        return session;
    }


    /// <inheritdoc />
    public async Task<SessionStatusResponse> GetSessionStatus(string code)
    {
        var session = await _sessionRepository.GetByCodeAsync(code)
            ?? throw new NotFoundException(nameof(Session), code);

        var currentUserId = _currentUserService.UserId 
            ?? throw new UnauthorizedAccessException();

        var sessionStatusResponse = new SessionStatusResponse
        {
            Status = session.Status
        };

        return sessionStatusResponse;
    }

    private async Task<List<QuizTiny>> BuildQuizzes(List<Guid> quizIds)
    {
        var quizzes = await _quizService.GetQuizzesByIds(quizIds);

        if (quizzes.Count != quizIds.Count)
        {
            throw new BadRequestException("One or more quizzes were not found.");
        }

        return quizzes.Select(x => x.ToTiny()).ToList();
    }

    private static string GenerateCode()
    {
        return Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
    }
}
