namespace QuizWorld.Infrastructure.Common.Options;

/// <summary>
/// Represents options for blob storage.
/// </summary>
public class BlobStorageOptions
{
    /// <summary>
    /// Gets or sets the connection string for the blob storage.
    /// </summary>
    public string ConnectionString { get; set; } = default!;

    /// <summary>
    /// Gets or sets the name of the container in the blob storage.
    /// </summary>
    public string ContainerName { get; set; } = default!;
}