using QuizWorld.Infrastructure.Interfaces;

namespace QuizWorld.Infrastructure.Common.Options;

/// <summary>
/// Represents options for the LLM (Large Language Model) service.
/// </summary>
public class LLMOptions
{
    /// <summary>
    /// If the LLM service is Azure OpenAI.
    /// </summary>
    public bool IsAzureOpenAI { get; set; }

    /// <summary>
    /// If the LLM service should use an assistant.
    /// </summary>
    public bool UseAssistant { get; set; }

    /// <summary>
    /// The Azure resource URL.
    /// </summary>
    public string? AzureResourceUrl { get; set; }

    /// <summary>
    /// The Azure API key.
    /// </summary>
    public string? AzureApiKey { get; set; }

    /// <summary>
    /// The OpenAI API key.
    /// </summary>
    public string? OpenAIApiKey { get; set; }

    /// <summary>
    /// The OpenAI model.
    /// </summary>
    public string Model { get; set; } = default!;

    /// <summary>
    /// The maximum number of generation attempts.
    /// </summary>
    public int MaxGenerationAttempts { get; set; }

    /// <summary>
    /// The assistant IDs for each content type.
    /// </summary>
    public Dictionary<GenerateContentType, string>? AssistantIds { get; set; }
}