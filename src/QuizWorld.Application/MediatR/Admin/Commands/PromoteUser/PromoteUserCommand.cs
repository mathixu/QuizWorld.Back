using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.MediatR.Common;
using System.Text.Json.Serialization;

namespace QuizWorld.Application.MediatR.Admin.Commands.PromoteUser;

/// <summary>
/// Represents the command for promoting a user.
/// </summary>
public class PromoteUserCommand : IQuizWorldRequest<ProfileResponse>
{
    /// <summary>
    /// Represents the user's id.
    /// </summary>
    [JsonIgnore]
    public Guid UserId { get; set; }

    /// <summary>
    /// Represents the role of the user.
    /// </summary>
    public string Role { get; set; } = default!;
}