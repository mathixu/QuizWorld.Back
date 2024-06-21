using QuizWorld.Domain.Common;

namespace QuizWorld.Domain.Entities;

public class UserHistory : BaseEntity
{
    /// <summary>
    /// Represents the user
    /// </summary>
    public UserTiny User { get; set; } = default!;

    /// <summary>
    /// Represents the quiz
    /// </summary>
    public QuizTiny Quiz { get; set; } = default!;

    /// <summary>
    /// Represents the date of the quiz
    /// </summary>
    public DateTime Date { get; set; }


    /// <summary>
    /// Represents the session Id
    /// </summary>
    public Guid SessionId { get; set; }

    /// <summary>
    /// Represents the results
    /// </summary>
    public UserSessionResult? Result { get; set; } = default!;
}
