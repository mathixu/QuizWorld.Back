using FluentValidation;

namespace QuizWorld.Application.MediatR.Users.Queries.SearchHistory;

public class SearchHistoryQueryValidator : AbstractValidator<SearchHistoryQuery>
{
    public SearchHistoryQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("The page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("The page size must be greater than 0.");
    }
}
