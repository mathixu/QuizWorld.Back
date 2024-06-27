using Azure;
using Azure.AI.OpenAI;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Options;
using QuizWorld.Infrastructure.Common.Helpers;
using QuizWorld.Infrastructure.Common.Options;
using QuizWorld.Infrastructure.Interfaces;

namespace QuizWorld.Infrastructure.Services;

public class OpenAIChatCompletion : ILLMService
{
    private readonly LLMOptions _options;
    private readonly OpenAIClient _client;
    private readonly TelemetryClient _telemetryClient;

    public OpenAIChatCompletion(IOptions<LLMOptions> options, TelemetryClient telemetryClient)
    {
        _options = options.Value;

        _client = _options.IsAzureOpenAI
            ? new OpenAIClient( new Uri(_options.AzureResourceUrl!), new AzureKeyCredential(_options.AzureApiKey!))
            : new OpenAIClient(_options.OpenAIApiKey!);

        _telemetryClient = telemetryClient;
    }

    /// <inheritdoc />
    public async Task<string> GenerateContent(GenerateContentType contentType, string input, string? fileUrl = null)
    { 
        var chatCompletionsOptions = new ChatCompletionsOptions()
        {
            DeploymentName = _options.Model,
            Messages = {
                new ChatRequestSystemMessage(SystemMessages.Messages[contentType]),
                new ChatRequestUserMessage(input)
            },
            Temperature = 1,
        };

        var response = await _client.GetChatCompletionsAsync(chatCompletionsOptions);

        var responseMessage = response.Value.Choices[0].Message;

        return responseMessage.Content;
    }
}
