using AutoMapper;
using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Users.Commands.AddPromotionToUser;

/// <summary>
/// Represents the command handler to add a promotion to a user.
/// </summary>
public class AddPromotionToUserCommandHandler(IUserRepository userRepository, 
                                                IPromotionRepository promotionRepository, 
                                                IMapper mapper) : IRequestHandler<AddPromotionToUserCommand, QuizWorldResponse<ProfileResponse>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPromotionRepository _promotionRepository = promotionRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<QuizWorldResponse<ProfileResponse>> Handle(AddPromotionToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        var promotion = await _promotionRepository.GetByIdAsync(request.PromotionId)
            ?? throw new NotFoundException(nameof(Promotion), request.PromotionId);

        if (user.Promotion?.Id == promotion.Id) 
            return QuizWorldResponse<ProfileResponse>.Failure("The user already has this promotion.");

        user.Promotion = promotion.ToTiny();

        var result = await _userRepository.AddPromotionAsync(user.Id, user.Promotion);

        return result 
            ? QuizWorldResponse<ProfileResponse>.Success(_mapper.Map<ProfileResponse>(user))
            : QuizWorldResponse<ProfileResponse>.Failure("Failed to add promotion to user.");
    }
}
