using AutoMapper;
using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Identity.Commands.Signup;

/// <summary>
/// The signup command which is used to create a new user.
/// </summary>
public class SignupCommandHandler(
    IUserRepository userRepository, 
    IHashService hashService, 
    IMapper mapper, 
    IIdentityService identityService) : IRequestHandler<SignupCommand, QuizWorldResponse<ProfileAndTokensResponse>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IHashService _hashService = hashService;
    private readonly IMapper _mapper = mapper;
    private readonly IIdentityService _identityService = identityService;

    public async Task<QuizWorldResponse<ProfileAndTokensResponse>> Handle(SignupCommand request, CancellationToken cancellationToken)
    {
        var alreadyExist = await _userRepository.GetByEmailAsync(request.Email);

        if (alreadyExist is not null)
            throw new AlreadyExistException("User with this email already exists.");

        var user = _mapper.Map<User>(request);

        user.HashedPassword = _hashService.Hash(request.Password);

        await _userRepository.AddAsync(user);

        var response = new ProfileAndTokensResponse
        {
            Profile = _mapper.Map<ProfileResponse>(user),
            Tokens = await _identityService.GenerateTokens(user)
        };

        return QuizWorldResponse<ProfileAndTokensResponse>.Success(response, 201);
    }
}
