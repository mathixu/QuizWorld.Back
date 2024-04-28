using FluentValidation;

namespace QuizWorld.Application.MediatR.Quizzes.Commands.CreateQuiz;

/// <summary>
/// Represents a validator for the <see cref="CreateQuizCommand"/> class.
/// </summary>
public class CreateQuizCommandValidator : AbstractValidator<CreateQuizCommand>
{
    public CreateQuizCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The name of the quiz is required.");

        RuleFor(x => x.TotalQuestions)
            .GreaterThan(0)
            .WithMessage("The total number of questions must be greater than 0.");

        RuleFor(x => x.SkillWeights)
            .NotEmpty()
            .WithMessage("At least one skill weight is required.")
            .Must(x => x.Values.Sum() == 100)
            .WithMessage("The sum of all skill weights must be 100.");
    }
}
