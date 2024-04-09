using FluentValidation;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Users.Queries.GetUsers;

/// <summary>
/// Represents the validator for the get users query.
/// </summary>
public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.");

        RuleFor(x => x.Search)
            .MaximumLength(255)
            .WithMessage("Search must be less than 50 characters.");

        RuleFor(x => x.Promotion)
            .MaximumLength(50)
            .WithMessage("Promotion must be less than 50 characters.");

        RuleFor(x => x.Role)
            .Must(x => string.IsNullOrEmpty(x) || x == AvailableRoles.Admin || x == AvailableRoles.Teacher || x == AvailableRoles.Player)
            .WithMessage("The role must be either 'Admin', 'Teacher', or 'Player'.");
    }
}
