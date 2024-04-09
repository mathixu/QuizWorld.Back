using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.Interfaces;

namespace QuizWorld.Application.MediatR.Identity.Commands.Refresh;

/// <summary>
/// The refresh command which is used to refresh the user's tokens.
/// </summary>
public class RefreshCommandHandler(IIdentityService identityService) : IRequestHandler<RefreshCommand, QuizWorldResponse<TokensResponse>>
{
    private readonly IIdentityService _identityService = identityService;

    public async Task<QuizWorldResponse<TokensResponse>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var response = await _identityService.RefreshTokens(request.Token);

        return QuizWorldResponse<TokensResponse>.Success(response, 200);
    }
}
