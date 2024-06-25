using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Sessions.Queries.GetSessionHistory;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Interfaces.Repositories;

public interface ISessionRepository
{
    /// <summary>Add a new session to the database.</summary>
    Task<bool> AddAsync(Session session);

    /// <summary>Get a session by its code.</summary>
    Task<Session?> GetByCodeAsync(string code);

    /// <summary>Get a session by its id.</summary>
    Task<Session?> GetByIdAsync(Guid sessionId);

    /// <summary>
    /// Update the status of a session.
    /// </summary>
    Task<bool> UpdateStatusAsync(Guid sessionId, SessionStatus status);

    /// <summary>
    /// Update the session.
    /// </summary>
    Task<bool> UpdateSessionAsync(Guid sessionId, Session session);

    /// <summary>
    /// Get the list of sessions.
    /// </summary>
    Task<PaginatedList<Session>> GetSessionHistoryAsync(Guid userId, GetSessionHistoryQuery query);
}
