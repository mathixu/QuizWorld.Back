using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetSession;

/// <summary>
/// Represents a query to get the status of a session.
/// </summary>
/// <param name="Code">The code of the session.</param>
public record GetSessionQuery(string Code) : IQuizWorldRequest<Session>;