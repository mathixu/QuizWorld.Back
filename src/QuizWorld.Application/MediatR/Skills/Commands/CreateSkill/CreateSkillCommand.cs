using QuizWorld.Application.MediatR.Common;
using QuizWorld.Domain.Entities;

namespace QuizWorld.Application.MediatR.Skills.Commands.CreateSkill;

/// <summary>
/// Represents a command to create a skill.
/// </summary>
/// <param name="Name">Represents the name of the skill.</param>
public record CreateSkillCommand(string Name) : IQuizWorldRequest<Skill>;