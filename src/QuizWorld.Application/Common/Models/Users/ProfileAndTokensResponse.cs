namespace QuizWorld.Application.Common.Models.Users;

/// <summary>
/// Represents the profile and tokens response.
/// </summary>
public class ProfileAndTokensResponse
{
    /// <summary>
    /// Represents the profile of the user.
    /// </summary>
    public ProfileResponse Profile { get; set; } = default!;

    /// <summary>
    /// Represents the tokens of the user.
    /// </summary>
    public TokensResponse Tokens { get; set; } = default!;
}
