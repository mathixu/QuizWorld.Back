using QuizWorld.Application.MediatR.Common;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetSessionStatus;

/// <summary>
/// Represents a query to get the status of a session.
/// </summary>
/// <param name="Code">The code of the session.</param>
public record GetSessionStatusQuery(string Code) : IQuizWorldRequest<SessionStatusResponse>;