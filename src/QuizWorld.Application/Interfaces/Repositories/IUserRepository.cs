using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Users.Queries.SearchUsers;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces.Repositories;

public interface IUserRepository
{
    /// <summary>Add a user.</summary>
    Task<bool> AddAsync(User user);

    /// <summary>Get a user by its id.</summary>
    Task<User?> GetByIdAsync(Guid id);

    /// <summary>Update a user.</summary>
    Task<bool> UpdateAsync(Guid userId, User user);

    /// <summary>Get a users by their ids.</summary>
    Task<List<User>> GetUsersByIdsAsync(List<Guid> userIds);

    /// <summary>Search for users.</summary>
    Task<PaginatedList<User>> SearchUsersAsync(SearchUsersQuery query);
}
