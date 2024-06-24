using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetUserHistories;

public class GetUserHistoriesQueryHandler(IUserHistoryRepository userHistoryRepository) : IRequestHandler<GetUserHistoriesQuery, QuizWorldResponse<PaginatedList<UserHistory>>>
{
    private readonly IUserHistoryRepository _userHistoryRepository = userHistoryRepository;

    public async Task<QuizWorldResponse<PaginatedList<UserHistory>>> Handle(GetUserHistoriesQuery request, CancellationToken cancellationToken)
    {
        var histories = await _userHistoryRepository.SearchUsersAsync(request);

        return QuizWorldResponse<PaginatedList<UserHistory>>.Success(histories);
    }
}
