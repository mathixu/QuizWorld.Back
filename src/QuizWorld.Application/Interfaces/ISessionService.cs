using QuizWorld.Application.MediatR.Sessions.Queries.GetSessionStatus;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces;

public interface ISessionService
{
    /// <summary>Creates a session with the given quiz ids.</summary>
    /// <param name="quizIds">Represents the ids of the quizzes.</param>
    /// <returns>Returns the created session.</returns>
    Task<Session> CreateSession(List<Guid> quizIds);

    /// <summary>Gets the status of a session.</summary>
    /// <param name="code">The code of the session.</param>
    /// <returns>Returns the status of the session.</returns>
    Task<SessionStatusResponse> GetSessionStatus(string code);
}
