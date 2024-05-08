using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Users.Queries.GetCurrentUser;

/// <summary>
/// Represents the query handler to get the current user.
/// </summary>
public class GetCurrentUserQueryHandler(ICurrentUserService currentUserService, IUserRepository userRepository) : IRequestHandler<GetCurrentUserQuery, QuizWorldResponse<User>>
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<QuizWorldResponse<User>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _currentUserService.User
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var user = await _userRepository.GetByIdAsync(currentUser.Id);

        // If the user is not found, create a new user in the database.
        if (user is null)
        {
            await _userRepository.AddAsync(currentUser);

            return QuizWorldResponse<User>.Success(currentUser, 201);
        } 
        // If the user is found, update the user in the database if the user has changed.
        else if (currentUser.FullName != user.FullName
                || currentUser.Email != user.Email
                || currentUser.Roles.SequenceEqual(user.Roles))
        {
            await _userRepository.UpdateAsync(currentUser.Id, currentUser);
        }

        return QuizWorldResponse<User>.Success(currentUser, 200);
    }
}
