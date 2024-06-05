using System.Text.Json.Serialization;

namespace QuizWorld.Application.Common.Models;

public class GeneratedQuestion
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = default!;

    [JsonPropertyName("answers")]
    public List<GeneratedAnswer> Answers { get; set; } = default!;

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("combinaison")]
    public List<List<int>>? Combinaisons { get; set; }
}
