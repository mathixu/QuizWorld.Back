using AutoMapper;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.MediatR.Quizzes.Commands.CreateQuiz;
using QuizWorld.Application.MediatR.Skills.Commands.CreateSkill;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Common.Mappings;

/// <summary>
/// Represents the mapping profile for requests.
/// </summary>
public class RequestProfile : Profile
{
    public RequestProfile()
    {
        CreateMap<CreateQuizCommand, Quiz>()
            .ForMember(dest => dest.NameNormalized, opt => opt.MapFrom(src => src.Name.ToNormalizedFormat()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Status.Pending))
            .ForMember(dest => dest.SkillWeights, opt => opt.Ignore());

        CreateMap<CreateSkillCommand, Skill>()
            .ForMember(dest => dest.NameNormalized, opt => opt.MapFrom(src => src.Name.ToNormalizedFormat()));
    }
}
