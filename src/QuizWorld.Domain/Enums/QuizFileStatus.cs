namespace QuizWorld.Domain.Enums;

/// <summary>
/// Represents the status of the quiz file.
/// </summary>
public enum QuizFileStatus
{
    /// <summary>Represents the status of the file when it is being uploaded.</summary>
    Uploading = 0,

    /// <summary>Represents the status of the file when it has been uploaded.</summary>
    Uploaded = 1,

    /// <summary>Represents the status of the file when it has failed to upload.</summary>
    Failed = 2
}
