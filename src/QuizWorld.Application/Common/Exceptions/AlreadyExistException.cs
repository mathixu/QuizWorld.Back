namespace QuizWorld.Application.Common.Exceptions;

/// <summary>
/// The exception that is thrown when an entity already exists. HTTP Status Code: 409.
/// </summary>
public class AlreadyExistException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AlreadyExistException"/> class. HTTP Status Code: 409.
    /// </summary>
    public AlreadyExistException() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlreadyExistException"/> class with a specified error message. HTTP Status Code: 409.
    /// </summary>
    /// <param name="message">
    /// The message that describes the error.
    /// </param>
    public AlreadyExistException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlreadyExistException"/> class with a specified entity name and key. HTTP Status Code: 409.
    /// </summary>
    /// <param name="entityName">
    /// The name of the entity that already exists.
    /// </param>
    /// <param name="entityKey">
    /// The key of the entity that already exists.
    /// </param>
    public AlreadyExistException(string entityName, string entityKey)
        : base($"{entityName} with {entityKey} already exist")
    {
    }
}
