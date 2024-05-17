using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Users.Queries.SearchUsers;

public class SearchUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<SearchUsersQuery, QuizWorldResponse<PaginatedList<UserTiny>>>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<QuizWorldResponse<PaginatedList<UserTiny>>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.SearchUsersAsync(request);

        return QuizWorldResponse<PaginatedList<UserTiny>>.Success(users.Map(x => x.ToTiny()), 200);
    }
}
