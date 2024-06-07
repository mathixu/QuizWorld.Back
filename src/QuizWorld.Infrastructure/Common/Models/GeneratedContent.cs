using QuizWorld.Domain.Common;
using QuizWorld.Infrastructure.Interfaces;

namespace QuizWorld.Infrastructure.Common.Models;

public class GeneratedContent : BaseAuditableEntity
{
    /// <summary>
    /// The content generated.
    /// </summary>
    public string Content { get; set; } = default!;

    /// <summary>
    /// The formatted content of the generated content.
    /// </summary>
    public string ContentFormatted { get; set; } = default!;

    /// <summary>
    /// The input used to generate the content.
    /// </summary>
    public string Input { get; set; } = default!;

    /// <summary>
    /// The type of the generated content.
    /// </summary>
    public GenerateContentType ContentType { get; set; }

    /// <summary>
    /// If the generated content has an error.
    /// </summary>
    public bool HasError { get; set; }

    /// <summary>
    /// The SkillId of the generated content.
    /// </summary>
    public Guid SkillId { get; set; }

    /// <summary>
    /// The QuizId of the generated content.
    /// </summary>
    public Guid QuizId { get; set; }

    /// <summary>
    /// The model used to generate the content.
    /// </summary>
    public string Model { get; set; } = default!;

    /// <summary>
    /// The attempt number of the generation.
    /// </summary>
    public int Attempt { get; set; } 
}
