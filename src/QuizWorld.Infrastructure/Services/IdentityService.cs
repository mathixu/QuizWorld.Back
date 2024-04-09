using AutoMapper;
using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Interfaces;

namespace QuizWorld.Infrastructure.Services;

/// <summary>
/// The identity service which is used to authenticate and refresh the user's tokens.
/// </summary>
public class IdentityService(
    IHashService hashService, 
    IUserRepository userRepository, 
    IMapper mapper, 
    IJwtProvider jwtProvider, 
    IRefreshTokenProvider refreshTokenProvider) : IIdentityService
{
    private readonly IHashService _hashService = hashService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IRefreshTokenProvider _refreshTokenProvider = refreshTokenProvider;

    /// <inheritdoc/>
    public async Task<ProfileAndTokensResponse> Authenticate(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email)
            ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!_hashService.Verify(password, user.HashedPassword))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var response = new ProfileAndTokensResponse
        {
            Profile = _mapper.Map<ProfileResponse>(user),
            Tokens = await GenerateTokens(user)
        };

        return response;
    }

    /// <inheritdoc/>
    public async Task<TokensResponse> RefreshTokens(string token)
    {
        var refreshToken = await _refreshTokenProvider.GetByToken(token);

        if (!refreshToken.IsValid)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        var user = await _userRepository.GetByIdAsync(refreshToken.User.Id)
            ?? throw new UnauthorizedAccessException("Invalid refresh token.");

        await _refreshTokenProvider.MarkAsUsed(refreshToken.Id);

        return await GenerateTokens(user);
    }

    /// <inheritdoc/>
    public async Task<TokensResponse> GenerateTokens(User user)
    {
        var refreshToken = await _refreshTokenProvider.Generate(user);

        var response = new TokensResponse
        {
            AccessToken = _jwtProvider.Generate(user),
            RefreshToken = refreshToken.Token
        };

        return response;
    }
}
