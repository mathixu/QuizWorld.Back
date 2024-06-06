using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Quizzes.Queries.GetQuizById;

/// <summary>
/// Represents a query for getting a quiz by its ID.
/// </summary>
public record GetQuizByIdQuery(Guid QuizId) : IQuizWorldRequest<Quiz>;