using MediatR;
using QuizWorld.Application.Common.Models;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Users.Queries.SearchHistory;

public class SearchHistoryQueryHandler(IUserHistoryRepository userHistoryRepository, ICurrentUserService currentUserService) : IRequestHandler<SearchHistoryQuery, QuizWorldResponse<PaginatedList<UserHistory>>>
{
    private readonly IUserHistoryRepository _userHistoryRepository = userHistoryRepository;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<QuizWorldResponse<PaginatedList<UserHistory>>> Handle(SearchHistoryQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("You're not authenticated.");

        var histories = await _userHistoryRepository.SearchHistoriesAsync(currentUserId, request);

        return QuizWorldResponse<PaginatedList<UserHistory>>.Success(histories);
    }
}
