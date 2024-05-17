using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using QuizWorld.Application.Interfaces;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Infrastructure.Services;

/// <summary>
/// Represents a service for Azure Blob Storage operations.
/// </summary>
public class BlobStorageService(IOptions<BlobStorageOptions> options) : IStorageService
{
    private readonly BlobServiceClient _blobServiceClient = new(options.Value.ConnectionString);
    private readonly string _containerName = options.Value.ContainerName;

    /// <inheritdoc />
    public async Task<string> UploadFileAsync(IFormFile file)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(file.FileName);

            await blobClient.UploadAsync(file.OpenReadStream(), true);

            return blobClient.Uri.AbsoluteUri;
        }
        catch
        {
            return string.Empty;
        }
    }
}
