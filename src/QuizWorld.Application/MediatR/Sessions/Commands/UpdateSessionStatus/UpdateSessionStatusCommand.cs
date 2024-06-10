using MediatR;
using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;
using QuizWorld.Domain.Enums;
using System.Text.Json.Serialization;

namespace QuizWorld.Application.MediatR.Sessions.Commands.UpdateSessionStatus;

public class UpdateSessionStatusCommand : IQuizWorldRequest<SessionTiny>
{
    /// <summary>
    /// Represents the session identifier.
    /// </summary>
    [JsonIgnore]
    public string? Code { get; set; } = default!;

    /// <summary>
    /// Represents the status of the session.
    /// </summary>
    public SessionStatus Status { get; set; }
}