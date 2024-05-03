using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Interfaces;

namespace QuizWorld.Infrastructure.Services;

/// <summary>Represents a service for interacting with Azure Key Vault.</summary>
public class KeyVaultService : IKeyVaultService
{
    private readonly SecretClient _client;

    public KeyVaultService()
    {
        var keyVaultUrl = Environment.GetEnvironmentVariable(Constants.ENV_VARIABLE_KEY_KEY_VAULT_URL);

        if (string.IsNullOrEmpty(keyVaultUrl))
        {
            throw new InvalidOperationException($"Key Vault URL is not set. [{Constants.ENV_VARIABLE_KEY_KEY_VAULT_URL}]");
        }

        _client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
    }

    /// <inheritdoc/>
    public async Task<string> GetSecretAsync(string secretName)
    {
        KeyVaultSecret secret = await _client.GetSecretAsync(secretName);
        return secret.Value;
    }
}
