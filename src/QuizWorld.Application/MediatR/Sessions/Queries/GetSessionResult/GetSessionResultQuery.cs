using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetSessionResult;

public record GetSessionResultQuery(string Code) : IQuizWorldRequest<UserSessionResult>;