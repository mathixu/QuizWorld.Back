namespace QuizWorld.Application.Common.Models.Users;

/// <summary>
/// Represents the tokens response.
/// </summary>
public class TokensResponse
{
    /// <summary>
    /// Represents the access token.
    /// </summary>
    public string AccessToken { get; set; } = default!;

    /// <summary>
    /// Represents the refresh token.
    /// </summary>
    public string RefreshToken { get; set; } = default!;
}
