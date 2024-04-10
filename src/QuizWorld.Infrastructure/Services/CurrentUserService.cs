using Microsoft.AspNetCore.Http;
using QuizWorld.Application.Interfaces;
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
}
