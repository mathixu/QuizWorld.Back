using QuizWorld.Domain.Common;

namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents a quiz file entity.
/// </summary>
public class QuizFile : BaseEntity
{

    /// <summary>
    /// Represents the date and time when the file was uploaded.
    /// </summary>
    public DateTime UploadedAt { get; set; }

    /// <summary>
    /// Represents the name of the file.
    /// </summary>
    public string FileName { get; set; } = default!;

    /// <summary>
    /// Represents the URL of the file.
    /// </summary>
    public string Url { get; set; } = default!;

    /// <summary>
    /// Represents the content type of the file.
    /// </summary>
    public string ContentType { get; set; } = default!;

    /// <summary>
    /// Represents the user that uploaded the file.
    /// </summary>
    public UserTiny UploadedBy { get; set; } = default!;
}
