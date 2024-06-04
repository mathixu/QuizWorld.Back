using Microsoft.Extensions.Options;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Infrastructure.Common.Options;
using System.Text.Json;

namespace QuizWorld.Presentation.OptionsSetup;

public class LLMOptionsSetup(IConfiguration configuration) : IConfigureOptions<LLMOptions>
{
    private readonly IConfiguration _configuration = configuration;

    public void Configure(LLMOptions options)
    {
        var serializedOptions = _configuration[Constants.KEY_VAULT_SECRET_LLM_SETTINGS];

        var deserializedOptions = JsonSerializer.Deserialize<LLMOptions>(serializedOptions);

        if (deserializedOptions.IsAzureOpenAI && (string.IsNullOrWhiteSpace(deserializedOptions.AzureResourceUrl) || string.IsNullOrWhiteSpace(deserializedOptions.AzureApiKey)))
            throw new ArgumentException("Azure resource URL and API key are required.");
        else if (!deserializedOptions.IsAzureOpenAI && string.IsNullOrWhiteSpace(deserializedOptions.OpenAIApiKey))
            throw new ArgumentException("OpenAI API key is required.");

        if (deserializedOptions.UseAssistant && deserializedOptions.AssistantIds == null)
            throw new ArgumentException("Assistant IDs are required.");

        options.IsAzureOpenAI = deserializedOptions.IsAzureOpenAI;
        options.AzureResourceUrl = deserializedOptions.AzureResourceUrl;
        options.AzureApiKey = deserializedOptions.AzureApiKey;
        options.OpenAIApiKey = deserializedOptions.OpenAIApiKey;
        options.Model = deserializedOptions.Model;
        options.UseAssistant = deserializedOptions.UseAssistant;
        options.AssistantIds = deserializedOptions.AssistantIds;
        options.MaxGenerationAttempts = deserializedOptions.MaxGenerationAttempts;
    }
}
