namespace QuizWorld.Infrastructure.Common.Options;

/// <summary>
/// Represents the JWT options.
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Represents the issuer of the token.
    /// </summary>
    public string Issuer { get; init; } = default!;

    /// <summary>
    /// Represents the audience of the token.
    /// </summary>
    public string Audience { get; init; } = default!;

    /// <summary>
    /// Represents the secret key used to sign the token.
    /// </summary>
    public string SecretKey { get; init; } = default!;

    /// <summary>
    /// Represents the access token expiration time in minutes.
    /// </summary>
    public int AccessTokenExpireInMinutes { get; init; }
}
