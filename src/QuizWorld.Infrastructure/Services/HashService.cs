using QuizWorld.Application.Interfaces;

namespace QuizWorld.Infrastructure.Services;

/// <summary>
/// Represents the hash service.
/// </summary>
public class HashService : IHashService
{
    /// <inheritdoc />
    public string Hash(string input)
    {
        return BCrypt.Net.BCrypt.HashPassword(input);
    }

    /// <inheritdoc />
    public bool Verify(string input, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(input, hash);
    }
}
