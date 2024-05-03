using Microsoft.AspNetCore.Http;

namespace QuizWorld.Application.Interfaces;

/// <summary>
/// Represents a service for storage operations.
/// </summary>
public interface IStorageService
{
    /// <summary>Uploads a file to the storage service.</summary>
    /// <param name="file">The file to upload.</param>
    /// <returns>The URL of the uploaded file.</returns>
    Task<string> UploadFileAsync(IFormFile file);
}
