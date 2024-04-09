using Microsoft.Extensions.Options;
using Moq;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;
using QuizWorld.Infrastructure.Common.Options;
using QuizWorld.Infrastructure.Services;

namespace QuizWorld.Infrastructure.Tests.Identity;

[TestClass]
public class RefreshTokenProviderTests
{
    private Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private RefreshTokenOptions _options;
    private RefreshTokenProvider _refreshTokenProvider;

    [TestInitialize]
    public void SetUp()
    {
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _options = new RefreshTokenOptions { ExpiresInDays = 7 };
        _refreshTokenProvider = new RefreshTokenProvider(_refreshTokenRepositoryMock.Object, Options.Create(_options));
    }

    [TestMethod]
    public async Task Generate_ShouldReturnNewTokenForValidUser()
    {
        // Arrange
        var user = new User() { Id = Guid.NewGuid(), Email = "test@test.com" };
        var expectedToken = new RefreshToken();

        _refreshTokenRepositoryMock.Setup(r => r.AddAsync(It.IsAny<RefreshToken>()))
                       .Returns(Task.FromResult(true))
                       .Callback<RefreshToken>(token => expectedToken = token);

        // Act
        var result = await _refreshTokenProvider.Generate(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreNotEqual(Guid.Empty, Guid.Parse(result.Token));
        Assert.AreEqual(DateTime.UtcNow.AddDays(_options.ExpiresInDays).Date, result.ExpiresAt.Date);
        
        Assert.IsNotNull(result.User);
        Assert.AreEqual(user.Id, result.User.Id);
        Assert.AreEqual(user.Email, result.User.Email);
        
        _refreshTokenRepositoryMock.Verify(r => r.AddAsync(It.IsAny<RefreshToken>()), Times.Once);
    }
}
