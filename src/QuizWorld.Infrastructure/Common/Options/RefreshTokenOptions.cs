namespace QuizWorld.Infrastructure.Common.Options;

/// <summary>
/// Represents the refresh token options.
/// </summary>
public class RefreshTokenOptions
{
    /// <summary>
    /// Represents the expiration time in days.
    /// </summary>
    public int ExpiresInDays { get; init; }
}
