using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Users.Queries.GetCurrentUser;

/// <summary>
/// Represents the query to get the current user.
/// </summary>
public record GetCurrentUserQuery() : IQuizWorldRequest<User>;