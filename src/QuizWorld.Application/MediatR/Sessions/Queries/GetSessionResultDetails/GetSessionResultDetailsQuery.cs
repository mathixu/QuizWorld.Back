using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetQuizResultDetails;

public record GetSessionResultDetailsQuery(Guid SessionId, Guid UserId) : IQuizWorldRequest<List<UserAnswer>>;
