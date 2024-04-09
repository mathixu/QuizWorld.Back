using QuizWorld.Domain.Common;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Application.Common.Models.Users;

/// <summary>
/// Represents the response with the user's profile.
/// </summary>
public class ProfileResponse : BaseAuditableEntity
{
    /// <summary>
    /// Represents the email of the user.
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Represents the first name of the user.
    /// </summary>
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// Represents the last name of the user.
    /// </summary>
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Represents the roles of the user.
    /// </summary>
    public string Role { get; set; } = default!;

    /// <summary>
    /// Represents the user's promotion.
    /// </summary>
    public PromotionTiny Promotion { get; set; } = default!;
}
