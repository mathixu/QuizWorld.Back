namespace QuizWorld.Application.Common.Exceptions;

/// <summary>
/// The exception that is thrown when an entity is not found. HTTP Status Code: 404.
/// </summary>
public class NotFoundException : Exception
{

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class. HTTP Status Code: 404.
    /// </summary>
    /// <param name="name">The name of the entity that was not found.</param>
    /// <param name="key">The key of the entity that was not found.</param>
    public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class. HTTP Status Code: 404.
    /// </summary>
    public NotFoundException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified error message. HTTP Status Code: 404.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NotFoundException(string message) : base(message) { }
}