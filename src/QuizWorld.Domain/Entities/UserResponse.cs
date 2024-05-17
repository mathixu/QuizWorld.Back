using QuizWorld.Domain.Common;

namespace QuizWorld.Domain.Entities;

public class UserResponse : BaseAuditableEntity
{
    /// <summary>Represents the user of the user response.</summary>
    public UserTiny User { get; set; } = default!;

    /// <summary>Represents the user session id of the user response.</summary>
    public Guid UserSessionId { get; set; }

    /// <summary>Represents responses</summary>
    public List<Responses> Responses { get; set; } = [];
}

public class Responses
{
    /// <summary>Represents the question of the user response.</summary>
    public QuestionMinimal Question { get; set; } = default!;

    /// <summary>Represents the answers of the user response.</summary>
    public List<AnswerTiny> Answers { get; set; } = [];

    /// <summary>Represents if the user response is correct.</summary>
    public bool IsCorrect { get; set; } = false;

    /// <summary>Represents the quiz id of the user response./// </summary>
    public Guid QuizId { get; set; }
}