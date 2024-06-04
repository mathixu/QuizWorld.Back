using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces;

/// <summary>
/// Interface for a question generator.
/// </summary>
public interface IQuestionGenerator
{
    /// <summary>Generate questions by skills.</summary>
    /// <param name="quizId">The quiz id.</param>
    /// <param name="skill">The skill.</param>
    /// <param name="totalQuestions">The total questions.</param>
    /// <param name="file">The quiz file.</param>
    /// <returns>A list of questions.</returns>
    Task<List<Question>> GenerateQuestionsBySkills(Guid quizId, SkillTiny skill, int totalQuestions, QuizFile? file = null);
    Task<List<Question>> GenerateQuestionsBySkills(Guid quizId, SkillTiny skill, int totalQuestions, float temperature);
}
