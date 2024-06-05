using System.Text.Json.Serialization;

namespace QuizWorld.Application.Common.Models;
public class GeneratedAnswer
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = default!;

    [JsonPropertyName("isCorrect")]
    public bool? IsCorrect { get; set; } // Nullable for combination type answers

    [JsonPropertyName("id")]
    public int? Id { get; set; } // Nullable for non-combination type answers
}
