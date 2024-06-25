﻿using FluentValidation;

namespace QuizWorld.Application.MediatR.Sessions.Queries.GetSessionHistory;

public class GetSessionHistoryQueryValidator : AbstractValidator<GetSessionHistoryQuery>
{
    public GetSessionHistoryQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("The page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("The page size must be greater than 0.");
    }
}