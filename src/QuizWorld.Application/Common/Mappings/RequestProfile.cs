using AutoMapper;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.MediatR.Identity.Commands.Signup;
using QuizWorld.Application.MediatR.Promotions.Commands.CreatePromotion;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Common.Mappings;

/// <summary>
/// Represents the mapping profile for requests.
/// </summary>
public class RequestProfile : Profile
{
    public RequestProfile()
    {
        CreateMap<SignupCommand, User>()
            .ForMember(dest => dest.EmailNormalized, opt => opt.MapFrom(src => src.Email.ToNormalizedFormat()))
            .ForMember(dest => dest.FirstNameNormalized, opt => opt.MapFrom(src => src.FirstName.ToNormalizedFormat()))
            .ForMember(dest => dest.LastNameNormalized, opt => opt.MapFrom(src => src.LastName.ToNormalizedFormat()))
            .ForMember(dest => dest.FullNameNormalized, opt => opt.MapFrom(src => src.FirstName.ToNormalizedFormat() + " " + src.LastName.ToNormalizedFormat()));

        CreateMap<CreatePromotionCommand, Promotion>()
            .ForMember(dest => dest.NameNormalized, opt => opt.MapFrom(src => src.Name.ToNormalizedFormat()));
    }
}
