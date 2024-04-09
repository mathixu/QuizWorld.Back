using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.Interfaces;

namespace QuizWorld.Application.MediatR.Identity.Commands.Login;

/// <summary>
/// The login command handler which is used to authenticate a user.
/// </summary>
public class LoginCommandHandler(IIdentityService identityService) : IRequestHandler<LoginCommand, QuizWorldResponse<ProfileAndTokensResponse>>
{
    private readonly IIdentityService _identityService = identityService;

    public async Task<QuizWorldResponse<ProfileAndTokensResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var response = await _identityService.Authenticate(request.Email, request.Password);

        return QuizWorldResponse<ProfileAndTokensResponse>.Success(response, 200);
    }
}
