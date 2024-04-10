using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.MediatR.Common;

namespace QuizWorld.Application.MediatR.Users.Queries.GetProfile;

/// <summary>
/// Represents the query to get the profile of the current user.
/// </summary>
public record GetProfileQuery() : IQuizWorldRequest<ProfileResponse>;