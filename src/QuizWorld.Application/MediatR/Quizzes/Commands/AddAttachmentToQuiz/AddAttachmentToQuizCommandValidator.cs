using FluentValidation;

namespace QuizWorld.Application.MediatR.Quizzes.Commands.AddAttachmentToQuiz;

public class AddAttachmentToQuizCommandValidator : AbstractValidator<AddAttachmentToQuizCommand>
{
    private readonly string[] _allowedExtensions = [".jpeg", ".jpg", ".png", ".gif", ".mp4", ".mov", ".pdf", ".txt", ".doc", ".docx"];

    public AddAttachmentToQuizCommandValidator()
    {
        RuleFor(x => x.Attachment)
            .NotNull()
            .WithMessage("Attachment is required.")
            .Must(x => x.Length > 0)
            .WithMessage("Attachment is required.")
            .Must(x => x.Length <= 10485760)
            .WithMessage("Attachment must be less than 10MB.")
            .Must(x => _allowedExtensions.Contains(Path.GetExtension(x.FileName).ToLower()))
            .WithMessage($"Attachment must be a valid file type: {string.Join(", ", _allowedExtensions)}");
    }
}
