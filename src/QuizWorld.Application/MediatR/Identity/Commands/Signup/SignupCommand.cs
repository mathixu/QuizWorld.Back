using QuizWorld.Application.Common.Models.Users;
using QuizWorld.Application.MediatR.Common;

namespace QuizWorld.Application.MediatR.Identity.Commands.Signup;

/// <summary>
/// The signup command which is used to create a new user.
/// </summary>
/// <param name="Email">The email of the user. Must be in a valid email format and not exceed 255 characters.</param>
/// <param name="FirstName">The first name of the user. Must not exceed 255 characters.</param>
/// <param name="LastName">The last name of the user. Must not exceed 255 characters.</param>
/// <param name="Password">The password of the user. Must be at least 6 characters long and not exceed 255 characters.</param>
public record SignupCommand(string Email, string FirstName, string LastName, string Password) : IQuizWorldRequest<ProfileResponse>;