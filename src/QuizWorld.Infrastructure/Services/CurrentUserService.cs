using Microsoft.AspNetCore.Http;
using QuizWorld.Application.Interfaces;
using QuizWorld.Domain.Entities;
using System.Security.Claims;
using Microsoft.Identity.Web;

namespace QuizWorld.Infrastructure.Services;

/// <summary>
/// Service used to get the current user.
/// </summary>
public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    /// <inheritdoc />
    public string? UserEmail => _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value;

    /// <inheritdoc />
    public Guid? UserId
    {
        get
        {
            var id = _httpContextAccessor.HttpContext?.User?.GetObjectId();

            if (string.IsNullOrEmpty(id))
                return null;

            if (!Guid.TryParse(id, out var userId))
                return null;

            return userId;
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
    public string? UserFullName => _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "name")?.Value;

    /// <inheritdoc />
    public string[]? UserRoles => _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();

    /// <inheritdoc />
    public User? User
    {
        get
        {
            if (!UserId.HasValue || string.IsNullOrEmpty(UserEmail) || string.IsNullOrEmpty(UserFullName) || UserRoles == null)
                return null;

            return new User
            {
                Id = UserId.Value,
                Email = UserEmail,
                FullName = UserFullName,
                Roles = UserRoles
            };
        }
    }

    /// <inheritdoc />
    public UserTiny? UserTiny
    {
        get
        {
            if (!UserId.HasValue || string.IsNullOrEmpty(UserFullName))
                return null;

            return new UserTiny
            {
                Id = UserId.Value,
                FullName = UserFullName
            };
        }
    }

    public User? ExtractUser(ClaimsPrincipal claimsPrincipal)
    {
        var id = claimsPrincipal.GetObjectId();

        if (string.IsNullOrEmpty(id))
            return null;

        if (!Guid.TryParse(id, out var userId))
            return null;

        var email = claimsPrincipal.FindFirst("preferred_username")?.Value;
        var fullName = claimsPrincipal.FindFirst("name")?.Value;
        var roles = claimsPrincipal.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(fullName) || roles == null)
            return null;

        return new User
        {
            Id = userId,
            Email = email,
            FullName = fullName,
            Roles = roles
        };
    }
}
