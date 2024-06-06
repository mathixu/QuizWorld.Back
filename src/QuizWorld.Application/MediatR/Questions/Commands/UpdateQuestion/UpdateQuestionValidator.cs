using FluentValidation;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.MediatR.Questions.Commands.UpdateQuestion
{
    public class UpdateQuestionValidator : AbstractValidator<UpdateQuestionCommand>
    {
        public UpdateQuestionValidator()
        {
            RuleFor(x => x.Question.Text)
                .NotEmpty()
                .WithMessage("The text of the question cannot be null");

            RuleFor(x => x.Question.Answers)
                .NotEmpty()
                .WithMessage("the question cannot have any answer.");

            RuleFor(x => x.Question.Answers.Where(a => a.IsCorrect.HasValue && a.IsCorrect.Value))
                .Must(answers => answers.Count() >= 2)
                .When(x => x.Question.Type == "multiple")
                .WithMessage("At least two answers are required for multiple choice questions.");

            RuleFor(x => x.Question.Answers.Where(a => a.IsCorrect.HasValue && a.IsCorrect.Value))
                .Must(answers => answers.Count() == 1)
                .WithMessage("Exactly one answer is required for a simple choice question.")
                .When(x => x.Question.Type == "simple");

            RuleForEach(x => x.Question.Answers)
               .Must(answer => answer.Id == null)
               .When(x => x.Question.Type != "combinaison")
               .WithMessage("Answer Ids must be null when the question type is not 'combinaison'.");

            RuleForEach(x => x.Question.Answers)
                .Must(answer => answer.Id != null)
                .When(x => x.Question.Type == "combinaison")
                .WithMessage("Answer Ids must not be null when the question type is 'combinaison'.");

            RuleFor(x => x.Question.Combinaisons)
                .Must(combinations => combinations != null)
                .When(x => x.Question.Type == "combinaison")
                .WithMessage("The 'combinaisons' property must be in the body when the question type is 'combinaison'.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Question.Combinaisons)
                        .Must((command, combinations) =>
                        {
                            if (combinations is null) 
                                if (command.Question.Type != "combinaison") return true;
                                else return false;

                            var answerIds = command.Question.Answers.Select(a => a.Id).ToList();
                            return combinations.All(combination => combination.All(id => answerIds.Contains(id)));
                        })
                        .WithMessage("All IDs in the combinations must correspond to the IDs in the Answers list.");
                });

            RuleFor(x => x.Question.Combinaisons)
                .Must(combinations => combinations == null)
                .When(x => x.Question.Type != "combinaison")
                .WithMessage("The 'combinaisons' property must be null when the question type is not 'combinaison'.");

        }
    }
}
