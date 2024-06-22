using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetSessionById;

public record GetSessionByIdQuery(Guid SessionId) : IQuizWorldRequest<Session>;