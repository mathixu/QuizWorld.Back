using QuizWorld.Infrastructure.Common.Models;

namespace QuizWorld.Infrastructure.Interfaces;

public interface IGeneratedContentRepository
{
    /// <summary>
    /// Add a new generated content to the database.
    /// </summary>
    /// <param name="content">
    /// The generated content to add.
    /// </param>
    public Task<bool> AddAsync(GeneratedContent content);
}
