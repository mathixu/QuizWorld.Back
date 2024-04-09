using AutoMapper;
using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Admin.Commands.PromoteUser;

/// <summary>
/// Represents the command handler for promoting a user.
/// </summary>
public class PromoteUserCommandHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<PromoteUserCommand, QuizWorldResponse<ProfileResponse>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<QuizWorldResponse<ProfileResponse>> Handle(PromoteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        if (user.Role == request.Role)
        {
            return QuizWorldResponse<ProfileResponse>.Failure($"User already has the role {request.Role}", 400);
        }

        await _userRepository.UpdateRole(user.Id, request.Role);

        user.Role = request.Role;
        
        return QuizWorldResponse<ProfileResponse>.Success(_mapper.Map<ProfileResponse>(user));
    }
}
