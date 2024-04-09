using AutoMapper;
using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.Interfaces.Repositories;

namespace QuizWorld.Application.MediatR.Users.Queries.GetUsers;

/// <summary>
/// Represents the query handler for getting users.
/// </summary>
public class GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUsersQuery, QuizWorldResponse<PaginatedList<ProfileResponse>>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<QuizWorldResponse<PaginatedList<ProfileResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetUsersAsync(request);

        var profileResponses = users.Map<ProfileResponse>(_mapper);

        return QuizWorldResponse<PaginatedList<ProfileResponse>>.Success(profileResponses);
    }
}
