using Microsoft.AspNetCore.Http;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;
using System.Security.Claims;

namespace QuizWorld.Infrastructure.Services;

/// <summary>
/// Service used to get the current user.
/// </summary>
public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    /// <inheritdoc />
    public string? UserEmail => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

    /// <inheritdoc />
    public Guid? UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null || !Guid.TryParse(userId, out var userIdGuid))
            {
                return null;
            }

            return userIdGuid;
        }
    }

    /// <inheritdoc />
    public string? UserToken
    {
        get
        {
            var header = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"];

            return header?.ToString().Split(" ").Last();
        }
    }

    /// <inheritdoc />
    public UserTiny? User
    {
        get
        {
            var email = UserEmail;
            var id = UserId;

            if (string.IsNullOrEmpty(email) || id is null)
                return null;

            return new UserTiny
            {
                Id = id.Value,
                Email = email
            };
        }
    }
}
