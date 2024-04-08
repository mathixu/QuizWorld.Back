using QuizWorld.Domain.Common;
using QuizWorld.Domain.Enums;

namespace QuizWorld.Domain.Entities;

/// <summary>
/// Represents a user entity.
/// </summary>
public class User : BaseAuditableEntity
{
    /// <summary>
    /// Represents the email of the user.
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Represents the email of the user in normalized form.
    /// </summary>
    public string EmailNormalized { get; set; } = default!;

    /// <summary>
    /// Represents the hashed password of the user.
    /// </summary>
    public string HashedPassword { get; set; } = default!;

    /// <summary>
    /// Represents the first name of the user.
    /// </summary>
    public string FirstName { get; set; } = default!;

    /// <summary>
    /// Represents the first name of the user in normalized form.
    /// </summary>
    public string FirstNameNormalized { get; set; } = default!;

    /// <summary>
    /// Represents the last name of the user.
    /// </summary>
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Represents the last name of the user in normalized form.
    /// </summary>
    public string LastNameNormalized { get; set; } = default!;

    /// <summary>
    /// Represents the roles of the user.
    /// </summary>
    public List<string> Roles { get; set; } = [AvailableRoles.Player];

    /// <summary>
    /// Represents the user's promotion.
    /// </summary>
    public PromotionTiny Promotion { get; set; } = default!;
}
