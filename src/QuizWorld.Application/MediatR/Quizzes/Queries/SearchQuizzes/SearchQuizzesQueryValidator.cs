using FluentValidation;

namespace QuizWorld.Application.MediatR.Quizzes.Queries.SearchQuizzes;

public class SearchQuizzesQueryValidator : AbstractValidator<SearchQuizzesQuery>
{
    public SearchQuizzesQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("The page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("The page size must be greater than 0.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("The status is not valid.");
    }
}