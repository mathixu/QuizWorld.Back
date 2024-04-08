namespace QuizWorld.Infrastructure.Common.Options;

/// <summary>
/// Represents the MongoDB options.
/// </summary>
public class MongoDbOptions
{
    /// <summary>
    /// Represents the connection string.
    /// </summary>
    public string ConnectionString { get; set; } = default!;

    /// <summary>
    /// Represents the database name.
    /// </summary>
    public string DatabaseName { get; set; } = default!;
}
