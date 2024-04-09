using QuizWorld.Domain.Common;

namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents the refresh token.
/// </summary>
public class RefreshToken : BaseEntity
{
    /// <summary>
    /// Represents the token.
    /// </summary>
    public string Token { get; set; } = default!;

    /// <summary>
    /// Represents the expiration date.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Boolean value indicating whether the token is used.
    /// </summary>
    public bool IsUsed { get; set; } = false;

    /// <summary>
    /// Boolean value indicating if token is valid.
    /// </summary>
    public bool IsValid => DateTime.UtcNow <= ExpiresAt && !IsUsed;

    /// <summary>
    /// Represents the user information.
    /// </summary>
    public UserTiny User { get; set; } = default!;
}