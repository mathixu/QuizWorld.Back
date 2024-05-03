namespace QuizWorld.Application.Common.Helpers;

/// <summary>
/// Represents a class containing constants.
/// </summary>
public static class Constants
{
    /// <summary>Represents the environment variable key for the Key Vault URL.</summary>
    public const string ENV_VARIABLE_KEY_KEY_VAULT_URL = "KEY_VAULT_URL";

    /// <summary>
    /// Represents the key for the Blob Storage settings in the Key Vault.
    /// </summary>
    public const string KEY_VAULT_SECRET_BLOB_STORAGE_SETTINGS = "BlobStorageSettings";

    /// <summary>
    /// Represents the key for the MongoDb settings in the Key Vault.
    /// </summary>
    public const string KEY_VAULT_SECRET_MONGO_DB_SETTINGS = "MongoDbSettings";
}
