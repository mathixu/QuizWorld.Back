namespace QuizWorld.Application.Interfaces;

/// <summary>
/// Interface for hashing service.
/// </summary>
public interface IHashService
{
    /// <summary>Hashes the input.</summary>
    /// <param name="input">The input.</param>
    /// <returns>The input hashed.</returns>
    string Hash(string input);

    /// <summary>Verifies the input against the hash.</summary>
    /// <param name="input">The input.</param>
    /// <param name="hash">The hash.</param>
    /// <returns>True if the input matches the hash, false otherwise.</returns>
    bool Verify(string input, string hash);
}
