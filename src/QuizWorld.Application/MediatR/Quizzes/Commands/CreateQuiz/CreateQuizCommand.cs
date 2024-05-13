using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Quizzes.Commands.CreateQuiz;

/// <summary>
/// Represents a command to create a quiz.
/// </summary>
public class CreateQuizCommand : IQuizWorldRequest<Quiz>
{
    /// <summary>Represents the title of the quiz.</summary>
    public string Name { get; set; } = default!;

    /// <summary>Represents the total number of questions in the quiz.</summary>
    public int TotalQuestions { get; set; }

    /// <summary>If we should display if the answer has multiple choices.</summary>
    public bool DisplayMultipleChoice { get; set; }

    /// <summary>If the quiz has a file.</summary>
    public bool HasFile { get; set; }

    /// <summary>Represents SkillWeights of the quiz. [SkillId, %]</summary>
    public Dictionary<Guid, int> SkillWeights { get; set; } = default!;

    /// <summary>If the quiz has personalized questions for each user.</summary>
    public bool PersonalizedQuestions { get; set; }

    /// <summary>Represents the users that have access to the quiz. (if PersonalizedQuestions is true)</summary>
    public List<Guid>? UserIds { get; set; } = default!;
}
