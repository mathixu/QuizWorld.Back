namespace QuizWorld.Application.Common.Models;

/// <summary>
/// The response model for the QuizWorld API.
/// </summary>
/// <typeparam name="TResponse">The type of the response data if successful.</typeparam>
public class QuizWorldResponse<TResponse>
{
    /// <summary>
    /// The data of the response if successful.
    /// </summary>
    public TResponse? Data { get; set; }

    /// <summary>
    /// Indicates whether the response is successful.
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// The status code of the response.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// The error message of the response if unsuccessful.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>Creates a successful response.</summary>
    /// <param name="data">The data of the response.</param>
    /// <param name="statusCode">The status code of the response.</param>
    /// <returns>The successful response.</returns>
    public static QuizWorldResponse<TResponse> Success(TResponse data, int statusCode = 200)
    {
        return new QuizWorldResponse<TResponse>
        {
            Data = data,
            IsSuccessful = true,
            StatusCode = statusCode
        };
    }

    /// <summary>Creates a failed response.</summary>
    /// <param name="errorMessage">The error message of the response.</param>
    /// <param name="statusCode">The status code of the response.</param>
    /// <returns>The failed response.</returns>
    public static QuizWorldResponse<TResponse> Failure(string errorMessage, int statusCode = 400)
    {
        return new QuizWorldResponse<TResponse>
        {
            IsSuccessful = false,
            StatusCode = statusCode,
            ErrorMessage = errorMessage
        };
    }
}
