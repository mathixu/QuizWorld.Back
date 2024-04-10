using AutoMapper;
using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;

namespace QuizWorld.Application.MediatR.Users.Queries.GetProfile;

/// <summary>
/// Represents the query handler to get the profile of the current user.
/// </summary>
public class GetProfileQueryHandler(ICurrentUserService currentUserService, IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetProfileQuery, QuizWorldResponse<ProfileResponse>>
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<QuizWorldResponse<ProfileResponse>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("The user is not authenticated.");

        var user = await _userRepository.GetByIdAsync(currentUserId)
            ?? throw new UnauthorizedAccessException("The user is not authenticated.");

        return QuizWorldResponse<ProfileResponse>.Success(_mapper.Map<ProfileResponse>(user));
    }
}
