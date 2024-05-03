namespace QuizWorld.Application.Interfaces;

/// <summary>
/// Represents a service for Azure Key Vault operations.
/// </summary>
public interface IKeyVaultService
{
    /// <summary>Get secret from Azure Key Vault.</summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <returns>The secret value.</returns>
    Task<string> GetSecretAsync(string secretName);
}
