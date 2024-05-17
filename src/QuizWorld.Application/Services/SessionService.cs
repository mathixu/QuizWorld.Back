using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Quizzes.Commands.StartQuiz;
using QuizWorld.Application.MediatR.Sessions.Queries.GetSessionStatus;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Services;

public class SessionService(IQuizService quizService, 
    ICurrentUserService currentUserService, 
    ISessionRepository sessionRepository,
    IUserSessionRepository userSessionRepository,
    ICurrentSessionService currentSessionService,
    IQuestionService questionService
    ) : ISessionService
{
    private readonly ISessionRepository _sessionRepository = sessionRepository;
    private readonly IQuizService _quizService = quizService;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IUserSessionRepository _userSessionRepository = userSessionRepository;
    private readonly ICurrentSessionService _currentSessionService = currentSessionService;
    private readonly IQuestionService _questionService = questionService;

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
        var session = await _sessionRepository.GetByCodeAsync(code);

        if (session is null)
            return new () { Status = SessionStatus.None };

        return new() { Status = session.Status };
    }

    /// <inheritdoc />
    public async Task<UserSession> AddUserSession(string code, string connectionId, User user)
    {
        var session = await _sessionRepository.GetByCodeAsync(code)
            ?? throw new NotFoundException(nameof(Session), code);

        if (session.Status != SessionStatus.Awaiting) 
            throw new BadRequestException("The session has already started.");

        var userSession = new UserSession(user.ToTiny(), session.ToTiny(), connectionId, session.CreatedBy.Id == user.Id);
        
        await _userSessionRepository.AddAsync(userSession);

        return userSession;
    }

    /// <inheritdoc />
    public async Task ChangeUserSessionStatus(string connectionId, UserSessionStatus status)
    {
        var userSession = await _userSessionRepository.GetByConnectionIdAsync(connectionId);

        if (userSession is null)
            return;

        userSession.Status = status;

        if (status == UserSessionStatus.DisconnectedWithError || status == UserSessionStatus.DisconnectedByUser)
        {
            userSession.DisconnectedAt = DateTime.UtcNow;
        }

        await _userSessionRepository.UpdateAsync(userSession);
    }

    /// <inheritdoc/>
    public async Task<StartQuizResponse> StartQuiz(Guid quizId)
    {
        var quiz = await _quizService.GetByIdAsync(quizId)
            ?? throw new NotFoundException(nameof(Quiz), quizId);

        var userSession = GetCurrentUserSession();

        var session = await _sessionRepository.GetByIdAsync(userSession.Session.Id)
            ?? throw new NotFoundException(nameof(Session), userSession.Id);

        if (session.Status != SessionStatus.Started)
            throw new BadRequestException("The session is not started yet.");

        if (!session.Quizzes.Any(x => x.Id == quizId))
            throw new BadRequestException("This quiz is not part of the session.");

        var currentUser = _currentUserService.User
            ?? throw new BadRequestException("You are not logged in.");

        var questions = await _questionService.GetCustomQuestions(quizId, currentUser.Id);

        var startQuizResponse = new StartQuizResponse
        {
            Questions = questions,
            Attachment = quiz.Attachment,
        };

        return startQuizResponse;
    }

    /// <inheritdoc/>
    public UserSession GetCurrentUserSession()
    {
        var currentUser = _currentUserService.User
            ?? throw new BadRequestException("You are not logged in.");

        var userSession = _currentSessionService.GetUserSessionByUser(currentUser)
            ?? throw new BadRequestException("You are not in a session.");

        return userSession;
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
