using FluentValidation;

namespace QuizWorld.Application.MediatR.Promotions.Queries.GetPromotions;

/// <summary>
/// Represents a query to get promotions.
/// </summary>
public class GetPromotionsQueryValidator : AbstractValidator<GetPromotionsQuery>
{
    public GetPromotionsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.");

        RuleFor(x => x.Search)
            .MaximumLength(255)
            .WithMessage("Search must be less than 255 characters.");
    }
}
