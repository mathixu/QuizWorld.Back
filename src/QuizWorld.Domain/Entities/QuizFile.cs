using QuizWorld.Domain.Common;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents a quiz file entity.
/// </summary>
public class QuizFile : BaseEntity
{
    /// <summary>Represents the status of the file.</summary>
    public QuizFileStatus Status { get; set; } = QuizFileStatus.Uploading;

    /// <summary>Represents the date and time when the file was uploaded.</summary>
    public DateTime? UploadedAt { get; set; }

    /// <summary>Represents the name of the file.</summary>
    public string? FileName { get; set; } = default!;

    /// <summary>Represents the URL of the file.</summary>
    public string? Url { get; set; } = default!;

    /// <summary>Represents the content type of the file.</summary>
    public string? ContentType { get; set; } = default!;
}


