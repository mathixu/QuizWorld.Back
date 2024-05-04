﻿using QuizWorld.Domain.Common;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents a question entity.
/// </summary>
public class Question : BaseAuditableEntity
{
    /// <summary>Represents the text of the question.</summary>
    public string Text { get; set; } = default!;

    /// <summary>Represents the status of the question.</summary>
    public Status Status { get; set; } = Status.Pending;

    /// <summary>Represents the type of the question.</summary>
    public QuestionType Type { get; set; }

    /// <summary>Represents the answers of the question.</summary>
    public List<Answer>? Answers { get; set; } = default!;

    /// <summary>Represents the combinaisons of answers. (if type was QuestionType.Combinaison)</summary>
    public List<List<Answer>>? Combinaisons { get; set; } = default!;

    /// <summary>Represents the quizId of the question.</summary>
    public Guid QuizId { get; set; }

    /// <summary>Represents the user for whom the question is customized.</summary>
    public UserTiny? QuestionFor { get; set; }
}
