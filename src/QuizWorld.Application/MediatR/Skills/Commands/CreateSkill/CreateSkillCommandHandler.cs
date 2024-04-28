using AutoMapper;
using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Skills.Commands.CreateSkill;

public class CreateSkillCommandHandler(ISkillRepository skillRepository, IMapper mapper) : IRequestHandler<CreateSkillCommand, QuizWorldResponse<Skill>>
{
    private readonly IMapper _mapper = mapper;
    private readonly ISkillRepository _skillRepository = skillRepository;

    public async Task<QuizWorldResponse<Skill>> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
    {
        var existingSkill = await _skillRepository.GetSkillByName(request.Name);

        if (existingSkill is not null)
            return QuizWorldResponse<Skill>.Failure($"Skill named {request.Name} already exists.", 409);

        var skill = _mapper.Map<Skill>(request);
        
        skill.Source = SkillSource.WebApp;

        await _skillRepository.AddAsync(skill);

        return QuizWorldResponse<Skill>.Success(skill, 201);
    }
}
