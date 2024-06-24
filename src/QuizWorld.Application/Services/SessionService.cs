using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Quizzes.Commands.StartQuiz;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Services;

public class SessionService(IQuizService quizService, 
    ICurrentUserService currentUserService, 
    ISessionRepository sessionRepository,
    IUserSessionRepository userSessionRepository,
    ICurrentSessionService currentSessionService,
    IQuestionService questionService,
    IUserHistoryRepository userHistoryRepository,
    IUserRepository userRepository,
    IUserAnswerRepository userAnswerRepository
    ) : ISessionService
{
    private readonly ISessionRepository _sessionRepository = sessionRepository;
    private readonly IQuizService _quizService = quizService;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IUserSessionRepository _userSessionRepository = userSessionRepository;
    private readonly ICurrentSessionService _currentSessionService = currentSessionService;
    private readonly IQuestionService _questionService = questionService;
    private readonly IUserHistoryRepository _userHistoryRepository = userHistoryRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserAnswerRepository _userAnswerRepository = userAnswerRepository;

    /// <inheritdoc />
    public async Task<Session> CreateSession(Guid quizId, SessionType sessionType)
    {
        var currentUser = _currentUserService.UserTiny 
            ?? throw new UnauthorizedAccessException();

        var quizz = await BuildQuizzes(quizId);

        string code;
        do
        {
            code = GenerateCode(sessionType == SessionType.Multiplayer ? 6 : 12);
        }
        while (await _sessionRepository.GetByCodeAsync(code) != null);

        var session = new Session
        {
            Quiz = quizz,
            Code = code,
            CreatedBy = currentUser,
            Status = SessionStatus.Awaiting,
            Type = sessionType
        };

        await _sessionRepository.AddAsync(session);

        return session;
    }


    /// <inheritdoc />
    public async Task<Session?> GetSessionByCode(string code)
    {
        return await _sessionRepository.GetByCodeAsync(code);
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

        await _userSessionRepository.UpdateAsync(userSession.Id, userSession);
    }

    /// <inheritdoc />
    public async Task<Session> UpdateSessionStatus(string code, SessionStatus status)
    {
        var session = await _sessionRepository.GetByCodeAsync(code)
            ?? throw new NotFoundException(nameof(Session), code);

        if (!_currentUserService.HasMinRole(Constants.MIN_TEACHER_ROLE) || session.CreatedBy.Id != _currentUserService.UserId)
            throw new UnauthorizedAccessException();

        if (status == SessionStatus.Started && session.Status != SessionStatus.Awaiting)
            throw new BadRequestException("The session has already started.");

        if (status == SessionStatus.Finished && session.Status != SessionStatus.Started)
            throw new BadRequestException("The session has not started yet.");

        if (status == SessionStatus.Started)
            session.StartingAt = DateTime.UtcNow;
        else if (status == SessionStatus.Finished)
            session.EndingAt = DateTime.UtcNow;

        session.Status = status;

        await _sessionRepository.UpdateSessionAsync(session.Id, session);

        session.Status = status;

        return session;
    }

    /// <inheritdoc/>
    public async Task<StartQuizResponse> StartQuiz(Guid quizId)
    {
        var quiz = await _quizService.GetByIdAsync(quizId)
            ?? throw new NotFoundException(nameof(Quiz), quizId);

        var userSession = await GetCurrentUserSession();

        var session = await _sessionRepository.GetByIdAsync(userSession.Session.Id)
            ?? throw new NotFoundException(nameof(Session), userSession.Id);

        if (session.Status != SessionStatus.Started)
            throw new BadRequestException("The session is not started yet.");

        if (session.Quiz.Id != quizId)
            throw new BadRequestException("This quiz is not part of the session.");

        var currentUser = _currentUserService.User
            ?? throw new BadRequestException("You are not logged in.");

        await AddNewUserHistory(quiz, currentUser, session.Id);

        var questions = await _questionService.GetCustomQuestions(quizId, currentUser.Id);

        var startQuizResponse = new StartQuizResponse
        {
            Questions = questions,
            Attachment = quiz.Attachment,
        };

        return startQuizResponse;
    }

    /// <inheritdoc />
    public async Task<List<UserAnswer>> GetSessionUserAnswers(Guid sessionId, Guid userId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId)
            ?? throw new NotFoundException(nameof(Session), sessionId);

        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new NotFoundException(nameof(User), userId);

        return await _userAnswerRepository.GetUserAnswers(sessionId, userId);
    }

    private async Task AddNewUserHistory(Quiz quiz, User user, Guid sessionId)
    {
        var userHistory = new UserHistory
        {
            Quiz = quiz.ToTiny(),
            Date = DateTime.UtcNow,
            User = user.ToTiny(),
            SessionId = sessionId
        };

        await _userHistoryRepository.AddAsync(userHistory);
    }

    /// <inheritdoc/>
    public async Task<UserSession> GetCurrentUserSession()
    {
        var currentUser = _currentUserService.User
            ?? throw new BadRequestException("You are not logged in.");

        var userSession = _currentSessionService.GetUserSessionByUser(currentUser)
            ?? throw new BadRequestException("You are not in a session.");

        var realUserSession = await _userSessionRepository.GetByIdAsync(userSession.Id)
            ?? throw new BadRequestException("The session was not found.");

        return realUserSession;
    }

    /// <inheritdoc />
    public async Task<UserSessionResult?> GetSessionResult(string code)
    {
        var user = _currentUserService.User
            ?? throw new BadRequestException("You are not logged in.");

        var userSession = await _userSessionRepository.GetLastBySessionCodeAndUserIdAsync(code, user.Id)
            ?? throw new BadRequestException("You are not in this session.");

        return userSession.Result;
    }

    private async Task<QuizTiny> BuildQuizzes(Guid quizId)
    {
        var quizz = await _quizService.GetByIdAsync(quizId);

        return quizz == null ? throw new BadRequestException("One or more quizzes were not found.") : quizz.ToTiny();
    }

    private static string GenerateCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();

        return new string(Enumerable.Repeat(chars, length)
                       .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
