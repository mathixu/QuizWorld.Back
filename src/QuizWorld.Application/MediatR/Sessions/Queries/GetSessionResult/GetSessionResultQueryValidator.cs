using FluentValidation;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetSessionResult;

public class GetSessionResultQueryValidator : AbstractValidator<GetSessionResultQuery>
{
    public GetSessionResultQueryValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required.");
    }
}
