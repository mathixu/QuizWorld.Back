using AutoMapper;
using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.Common.Mappings;

/// <summary>
/// Represents the mapping profile for responses.
/// </summary>
public class ResponseProfile : Profile
{
    public ResponseProfile()
    {
        CreateMap<User, ProfileResponse>();
    }
}
