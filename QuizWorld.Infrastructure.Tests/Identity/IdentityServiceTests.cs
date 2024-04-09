using AutoMapper;
using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Interfaces;
using QuizWorld.Infrastructure.Services;

namespace QuizWorld.Infrastructure.Tests.Identity;

[TestClass]
public class IdentityServiceTests
{
    private Mock<IHashService> _mockHashService;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IMapper> _mockMapper;
    private Mock<IJwtProvider> _mockJwtProvider;
    private Mock<IRefreshTokenProvider> _mockRefreshTokenProvider;
    private IdentityService _identityService;

    [TestInitialize]
    public void SetUp()
    {
        _mockHashService = new Mock<IHashService>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockJwtProvider = new Mock<IJwtProvider>();
        _mockRefreshTokenProvider = new Mock<IRefreshTokenProvider>();

        _identityService = new IdentityService(
            _mockHashService.Object,
            _mockUserRepository.Object,
            _mockMapper.Object,
            _mockJwtProvider.Object,
            _mockRefreshTokenProvider.Object);
    }

    #region Authenticate Method Tests
    [TestMethod]
    public async Task Authenticate_Success()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var user = new User { Email = email };
        var tokens = new ProfileAndTokensResponse {  };

        _mockUserRepository.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync(user);
        _mockHashService.Setup(service => service.Verify(password, user.HashedPassword)).Returns(true);
        _mockMapper.Setup(mapper => mapper.Map<ProfileResponse>(user)).Returns(new ProfileResponse { Email = user.Email });
        _mockJwtProvider.Setup(provider => provider.Generate(user)).Returns("token");
        _mockRefreshTokenProvider.Setup(provider => provider.Generate(user)).ReturnsAsync(new RefreshToken() { Token = Guid.NewGuid().ToString() });

        // Act
        var result = await _identityService.Authenticate(email, password);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(user.Email, result.Profile.Email);
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedAccessException))]
    public async Task Authenticate_InvalidEmail()
    {
        // Arrange
        var invalidEmail = "invalid@example.com";
        var password = "password123";

        _mockUserRepository.Setup(repo => repo.GetByEmailAsync(invalidEmail)).ReturnsAsync(null as User);

        // Act
        await _identityService.Authenticate(invalidEmail, password);

        // Assert is handled by ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedAccessException))]
    public async Task Authenticate_InvalidPassword()
    {
        // Arrange
        var email = "test@example.com";
        var invalidPassword = "wrongPassword";
        var user = new User { Email = email };

        _mockUserRepository.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync(user);
        _mockHashService.Setup(service => service.Verify(invalidPassword, user.HashedPassword)).Returns(false);


        // Act
        await _identityService.Authenticate(email, invalidPassword);

        // Assert is handled by ExpectedException
    }

    #endregion

    #region RefreshTokens Method Tests

    [TestMethod]
    public async Task RefreshTokens_Success()
    {
        // Arrange
        var validToken = "validToken123";
        var user = new User { Id = Guid.NewGuid() };
        var refreshToken = new RefreshToken
        {
            Token = validToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsUsed = false,
            User = new UserTiny { Id = user.Id }
        };

        _mockRefreshTokenProvider.Setup(p => p.GetByToken(validToken)).ReturnsAsync(refreshToken);
        _mockUserRepository.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
        _mockJwtProvider.Setup(provider => provider.Generate(user)).Returns("token");
        _mockRefreshTokenProvider.Setup(provider => provider.Generate(user)).ReturnsAsync(new RefreshToken() { Token = Guid.NewGuid().ToString() });
        
        // Act
        var result = await _identityService.RefreshTokens(validToken);

        // Assert
        Assert.IsNotNull(result);
        _mockRefreshTokenProvider.Verify(p => p.MarkAsUsed(refreshToken.Id), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedAccessException))]
    public async Task RefreshTokens_InvalidToken()
    {
        // Arrange
        var invalidToken = "invalidToken123";
        var refreshToken = new RefreshToken
        {
            Token = invalidToken,
            IsUsed = true
        };

        _mockRefreshTokenProvider.Setup(p => p.GetByToken(invalidToken)).ReturnsAsync(refreshToken);

        // Act
        await _identityService.RefreshTokens(invalidToken);

        // Assert est géré par ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(UnauthorizedAccessException))]
    public async Task RefreshTokens_InvalidUser()
    {
        // Arrange
        var validToken = "validToken123";
        var refreshToken = new RefreshToken
        {
            Token = validToken,
            IsUsed = false,
            User = new UserTiny { Id = Guid.NewGuid() }
        };

        _mockRefreshTokenProvider.Setup(p => p.GetByToken(validToken)).ReturnsAsync(refreshToken);
        _mockUserRepository.Setup(r => r.GetByIdAsync(refreshToken.User.Id)).ReturnsAsync((User)null);

        // Act
        await _identityService.RefreshTokens(validToken);

        // Assert est géré par ExpectedException
    }

    #endregion

    #region GenerateTokens Method Tests

    [TestMethod]
    public async Task GenerateTokens_Success()
    {
        // Arrange
        var user = new User {  };
        var refreshToken = new RefreshToken
        {
            Token = "refreshToken123",
        };
        var accessToken = "accessToken123";

        _mockRefreshTokenProvider.Setup(p => p.Generate(user)).ReturnsAsync(refreshToken);
        _mockJwtProvider.Setup(p => p.Generate(user)).Returns(accessToken);

        // Act
        var result = await _identityService.GenerateTokens(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(accessToken, result.AccessToken);
        Assert.AreEqual(refreshToken.Token, result.RefreshToken);
    }

    #endregion
}
