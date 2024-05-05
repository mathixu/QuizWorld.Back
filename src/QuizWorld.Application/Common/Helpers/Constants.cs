namespace QuizWorld.Application.Common.Helpers;

/// <summary>
/// Represents a class containing constants.
/// </summary>
public static class Constants
{
    /// <summary>Represents the environment variable key for the Key Vault URL.</summary>
    public const string ENV_VARIABLE_KEY_KEY_VAULT_URL = "KEY_VAULT_URL";



    /// <summary>Represents the key for the Blob Storage settings in the Key Vault.</summary>
    public const string KEY_VAULT_SECRET_BLOB_STORAGE_SETTINGS = "BlobStorageSettings";

    /// <summary>Represents the key for the MongoDb settings in the Key Vault.</summary>
    public const string KEY_VAULT_SECRET_MONGO_DB_SETTINGS = "MongoDbSettings";



    /// <summary>Represents the key for the AzureAd settings in the Key Vault.</summary>
    public const string KEY_VAULT_SECRET_AZURE_AD_SETTINGS = "AzureAd";

    /// <summary>Represents the key for the AzureAd Scopes in the Key Vault.</summary>
    public const string KEY_VAULT_SECRET_AZURE_AD_SCOPES = "AzureAd:Scopes:ReadWrite";


    /// <summary>
    /// Represents the role of an admin.
    /// </summary>
    public const string ADMIN_ROLE = "Admin";

    /// <summary>
    /// Represents the role of a teacher.
    /// </summary>
    public const string TEACHER_ROLE = "Teacher";

    /// <summary>
    /// Represents the role of a student.
    /// </summary>
    public const string STUDENT_ROLE = "Student";

    /// <summary>Used to represent the minimum role required for a student limited endpoint.</summary>
    public const string MIN_STUDENT_ROLE = $"{STUDENT_ROLE},{TEACHER_ROLE},{ADMIN_ROLE}";

    /// <summary>Used to represent the minimum role required for a teacher limited endpoint.</summary>
    public const string MIN_TEACHER_ROLE = $"{TEACHER_ROLE},{ADMIN_ROLE}";

    /// <summary>Used to represent the minimum role required for an admin limited endpoint.</summary>
    public const string MIN_ADMIN_ROLE = $"{ADMIN_ROLE}";
   

}
