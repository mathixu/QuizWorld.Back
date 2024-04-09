namespace QuizWorld.Application.Common.Exceptions;

/// <summary>
/// The exception that is thrown when a bad request is made. HTTP Status Code: 400.
/// </summary>
public class BadRequestException : Exception
{

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class. HTTP Status Code: 400.
    /// </summary>
    public BadRequestException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with a specified error message. HTTP Status Code: 400.
    /// </summary>
    /// <param name="message">
    /// The message that describes the error.
    /// </param>
    public BadRequestException(string message) : base(message) { }
}
