using FluentValidation;
using MediatR;
using QuizWorld.Application.Common.Exceptions;
using QuizWorld.Application.Common.Models;
using System.Reflection;

namespace QuizWorld.Application.MediatR.Common;

/// <summary>
/// The exception handling behavior for the MediatR pipeline which handles exceptions and returns a QuizWorldResponse with the error message and error code.
/// </summary>
public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly Dictionary<Type, int> _errorCodes;
    public ExceptionHandlingBehavior()
    {
        _errorCodes = new() {
            { typeof(ValidationException), 400 },
            { typeof(BadRequestException), 400 },
            { typeof(UnauthorizedAccessException), 401 },
            { typeof(ForbiddenAccessException), 403 },
            { typeof(NotFoundException), 404 },
            { typeof(AlreadyExistException), 409 },
        };
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var type = typeof(TResponse);

            if (IsQuizWorldResponse(type))
            {
                // Extract the type argument of the generic QuizWorldResponse<>
                var responseType = type.GetGenericArguments()[0];

                var failureMethod = GetFailureMethod(responseType);

                // Invoke the 'Failure' method with the exception message and error code
                var responseInstance = failureMethod?.Invoke(null, new object[] { ex.Message, GetErrorCode(ex) });

                return (TResponse)responseInstance!;
            }

            throw;
        }
    }

    /// <summary>Check if the type is a QuizWorldResponse<> type.</summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is a QuizWorldResponse<> type; otherwise, false.</returns>
    private static bool IsQuizWorldResponse(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(QuizWorldResponse<>);

    /// <summary>Get the 'Failure' method of the QuizWorldResponse<> type.</summary>
    /// <param name="responseType">The type argument of the QuizWorldResponse<> type.</param>
    /// <returns>The 'Failure' method of the QuizWorldResponse<> type.</returns>
    /// <exception cref="InvalidOperationException">The 'Failure' method could not be found on the QuizWorldResponse<> type.</exception>
    private static MethodInfo GetFailureMethod(Type responseType) => typeof(QuizWorldResponse<>).MakeGenericType(responseType).GetMethod("Failure") ?? throw new InvalidOperationException("The 'Failure' method could not be found on the DiAuthResponse<> type");

    /// <summary>Get the error code based on the exception type.</summary>
    /// <param name="ex">The exception.</param>
    /// <returns>The error code.</returns>
    private int GetErrorCode(Exception ex) => _errorCodes.ContainsKey(ex.GetType()) ? _errorCodes[ex.GetType()] : 500;
}
