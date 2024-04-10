using QuizWorld.Domain.Common;
using QuizWorld.Domain.Enums;
using System.Text.Json.Serialization;

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
    [JsonIgnore]
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
    [JsonIgnore]
    public string FirstNameNormalized { get; set; } = default!;

    /// <summary>
    /// Represents the last name of the user.
    /// </summary>
    public string LastName { get; set; } = default!;

    /// <summary>
    /// Represents the last name of the user in normalized form.
    /// </summary>
    [JsonIgnore]
    public string LastNameNormalized { get; set; } = default!;

    /// <summary>
    /// Represents the full name of the user.
    /// </summary>
    [JsonIgnore]
    public string FullNameNormalized { get; set; } = default!;

    /// <summary>
    /// Represents the role of the user.
    /// </summary>
    public string Role { get; set; } = AvailableRoles.Player;

    /// <summary>
    /// Represents the user's promotion.
    /// </summary>
    public PromotionTiny Promotion { get; set; } = default!;
}

/// <summary>
/// Represents a user entity with minimal information.
/// </summary>
public class UserTiny : BaseEntity
{
    /// <summary>
    /// Represents the email of the user.
    /// </summary>
    public string Email { get; set; } = default!;
}

/// <summary>
/// Extension methods for the User entity.
/// </summary>
public static class UserExtensions
{
    /// <summary>
    /// Converts a User entity to a UserTiny entity.
    /// </summary>
    /// <param name="user">The User entity to convert.</param>
    /// <returns>The UserTiny entity.</returns>
    public static UserTiny ToTiny(this User user)
    {
        return new UserTiny
        {
            Id = user.Id,
            Email = user.Email
        };
    }
}