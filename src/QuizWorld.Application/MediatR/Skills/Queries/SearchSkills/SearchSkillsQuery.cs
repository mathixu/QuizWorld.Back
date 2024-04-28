using QuizWorld.Application.Common.Models;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Skills.Queries.SearchSkills;

/// <summary>
/// Represents a query to search for skills.
/// </summary>
public class SearchSkillsQuery : PaginationQuery, IQuizWorldRequest<PaginatedList<SkillTiny>>
{
    /// <summary>
    /// Represents the name of the skill.
    /// </summary>
    public string? Name { get; set; } = default!;
}
