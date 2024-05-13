using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces.Repositories;

public interface ISessionRepository
{
    /// <summary>Add a new session to the database.</summary>
    Task<bool> AddAsync(Session session);

    /// <summary>Get a session by its code.</summary>
    Task<Session?> GetByCodeAsync(string code);
}
