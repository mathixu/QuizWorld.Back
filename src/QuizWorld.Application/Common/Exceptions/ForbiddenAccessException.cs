namespace QuizWorld.Application.Common.Exceptions;

/// <summary>
/// The exception that is thrown when an access is forbidden. HTTP Status Code: 403.
/// </summary>
public class ForbiddenAccessException : Exception
{

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenAccessException"/> class. HTTP Status Code: 403.
    /// </summary>
    public ForbiddenAccessException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenAccessException"/> class with a specified error message. HTTP Status Code: 403.
    /// </summary>
    /// <param name="message">
    /// The message that describes the error.
    /// </param>
    public ForbiddenAccessException(string message) : base(message) { }
}
