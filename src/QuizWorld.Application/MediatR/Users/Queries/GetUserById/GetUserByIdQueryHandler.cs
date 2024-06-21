using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Application.MediatR.Users.Queries.GetUserById;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Quizzes.Queries.GetQuizById;

public class GetUserByIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, QuizWorldResponse<UserTiny>>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<QuizWorldResponse<UserTiny>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId)
            ?? throw new NotFoundException(nameof(Quiz), request.UserId);

        return QuizWorldResponse<UserTiny>.Success(user.ToTiny());
    }
}
