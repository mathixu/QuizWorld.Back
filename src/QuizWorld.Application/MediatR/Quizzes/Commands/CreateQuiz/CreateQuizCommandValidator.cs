using FluentValidation;
using QuizWorld.Application.Interfaces;

namespace QuizWorld.Application.MediatR.Quizzes.Commands.CreateQuiz;

/// <summary>
/// Represents a validator for the <see cref="CreateQuizCommand"/> class.
/// </summary>
public class CreateQuizCommandValidator : AbstractValidator<CreateQuizCommand>
{
    public CreateQuizCommandValidator(ICurrentUserService currentUserService)
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

        RuleFor(x => x.UserIds)
            .NotEmpty()
            .When(x => x.PersonalizedQuestions)
            .WithMessage("At least one user is required for personalized questions.");

        When(x => x.PersonalizedQuestions && x.UserIds != null, () =>
        {
            RuleFor(x => x.UserIds)
                .Must(x => x!.Distinct().Count() == x.Count)
                .WithMessage("The list of user ids must be unique.");

            var currentUserId = currentUserService.UserId;
            RuleFor(x => x.UserIds)
                .Must(x => !x!.Contains(currentUserId ?? Guid.Empty))
                .WithMessage("The owner of the quiz cannot be included in the list of user ids.");
        });
    }
}
