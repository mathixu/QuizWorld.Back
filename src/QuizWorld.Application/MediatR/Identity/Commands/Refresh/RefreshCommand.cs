using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.MediatR.Common;

namespace QuizWorld.Application.MediatR.Identity.Commands.Refresh;

/// <summary>The refresh command which is used to refresh the user's tokens.</summary>
/// <param name="Token">The token which is used to refresh the user's tokens. </param>
public record RefreshCommand(string Token) : IQuizWorldRequest<TokensResponse>;