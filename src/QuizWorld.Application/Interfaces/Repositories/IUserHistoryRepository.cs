using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Users.Queries.SearchHistory;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces.Repositories;

public interface IUserHistoryRepository
{
    /// <summary>
    /// Add a new history to the database.
    /// </summary>
    Task<bool> AddAsync(UserHistory userHistory);

    /// <summary>
    /// Add user result to history.
    /// </summary>
    Task<bool> AddUserResultAsync(Guid userId, Guid sessionId, UserSessionResult result);

    /// <summary>
    /// Get User history
    /// </summary>
    Task<PaginatedList<UserHistory>> SearchHistoriesAsync(Guid userId, SearchHistoryQuery query);
}
