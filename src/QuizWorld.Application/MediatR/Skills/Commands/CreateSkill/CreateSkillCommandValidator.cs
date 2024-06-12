using FluentValidation;

namespace QuizWorld.Application.MediatR.Skills.Commands.CreateSkill;

public class CreateSkillCommandValidator : AbstractValidator<CreateSkillCommand>
{
    public CreateSkillCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The name of the skill is required.");
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("The description of the skill is required.");
    }
}
