using FluentValidation;

namespace QuizWorld.Application.MediatR.Questions.Commands.AnswerQuestion;

public class AnswerQuestionCommandValidator : AbstractValidator<AnswerQuestionCommand>
{
    public AnswerQuestionCommandValidator()
    {
        RuleFor(x => x.AnswerIds)
            .NotEmpty();

        RuleFor(x => x.AnswerIds)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("The answer ids must be unique.");
    }
}
