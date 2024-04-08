using MediatR;
using QuizWorld.Application.Common.Models;

namespace QuizWorld.Application.MediatR.Common;

/// <summary>
/// Represents the abstract request for IRequest for QuizWorldResponse.
/// </summary>
public interface IQuizWorldRequest<TResponse> : IRequest<QuizWorldResponse<TResponse>>;