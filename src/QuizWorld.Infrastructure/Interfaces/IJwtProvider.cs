using QuizWorld.Domain.Entities;

namespace QuizWorld.Infrastructure.Interfaces;

/// <summary>
/// Represents the JWT provider.
/// </summary>
public interface IJwtProvider
{
    /// <summary>Generates the JWT token.</summary>
    /// <param name="user">The user information used to generate the token.</param>
    /// <returns>The generated token.</returns>
    public string Generate(User user);
}
