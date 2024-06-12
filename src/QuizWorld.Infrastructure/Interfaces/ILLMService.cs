namespace QuizWorld.Infrastructure.Interfaces;

/// <summary>
/// Interface for the LLM (Large Language Model) service.
/// </summary>
public interface ILLMService
{
    /// <summary>Generate content based on the content type and input.</summary>
    /// <param name="contentType">The type of content to generate.</param>
    /// <param name="input">The input to generate content from.</param>
    /// <param name="fileUrl">The file URL to generate content from.</param>
    /// <returns>The generated content.</returns>
    Task<string> GenerateContent(GenerateContentType contentType, string input, string? fileUrl = null);
}

/// <summary>
/// Enum for specifying the type of content to generate.
/// </summary>
public enum GenerateContentType
{
    /// <summary>
    /// Unknown content type.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Generate questions.
    /// </summary>
    QuestionsBySkills = 1,

    /// <summary>
    /// Regenerate question.
    /// </summary>
    RegenerateQuestion = 2,
}
