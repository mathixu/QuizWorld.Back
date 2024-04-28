using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Skills.Queries.SearchSkills;

public class SearchSkillsQueryHandler(ISkillRepository skillRepository) : IRequestHandler<SearchSkillsQuery, QuizWorldResponse<PaginatedList<SkillTiny>>>
{
    private readonly ISkillRepository _skillRepository = skillRepository;

    public async Task<QuizWorldResponse<PaginatedList<SkillTiny>>> Handle(SearchSkillsQuery request, CancellationToken cancellationToken)
    {
        var skills = await _skillRepository.SearchSkillsAsync(request);

        var skillsTiny = skills.Map(x => x.ToTiny());

        return QuizWorldResponse<PaginatedList<SkillTiny>>.Success(skillsTiny, 200);
    }
}
