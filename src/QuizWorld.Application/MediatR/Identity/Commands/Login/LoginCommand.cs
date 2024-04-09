using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.MediatR.Common;

namespace QuizWorld.Application.MediatR.Identity.Commands.Login;

/// <summary>
/// The login command which is used to authenticate a user.
/// </summary>
/// <param name="Email">The email of the user.</param>
/// <param name="Password">The password of the user.</param>
public record LoginCommand(string Email, string Password) : IQuizWorldRequest<ProfileAndTokensResponse>;