using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Skills.Queries.SearchSkills;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Interfaces.Repositories;

public interface ISkillRepository
{
    /// <summary>Add a new skill to the database.</summary>
    Task<bool> AddAsync(Skill skill);

    /// <summary>Get skills by their id.</summary>
    Task<List<Skill>> GetSkillsByIdsAsync(IEnumerable<Guid> skillIds);

    /// <summary>Get a skill by its name.</summary>
    Task<Skill?> GetSkillByName(string name);

    /// <summary>Search for skills by their name.</summary>
    Task<PaginatedList<Skill>> SearchSkillsAsync(SearchSkillsQuery query);

    /// <summary>
    /// Get a skill by id.
    /// </summary>
    /// <param name="id">the skill id.</param>
    /// <returns>the object Skill.</returns>
    Task<Skill> GetById(Guid id);
}
