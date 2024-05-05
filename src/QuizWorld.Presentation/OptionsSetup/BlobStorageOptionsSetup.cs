using Microsoft.Extensions.Options;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Infrastructure.Common.Options;
using System.Text.Json;

namespace QuizWorld.Presentation.OptionsSetup;

public class BlobStorageOptionsSetup(IConfiguration configuration) : IConfigureOptions<BlobStorageOptions>
{
    private readonly IConfiguration _configuration = configuration;

    public void Configure(BlobStorageOptions options)
    {
        var serializedOptions = _configuration[Constants.KEY_VAULT_SECRET_BLOB_STORAGE_SETTINGS];

        var deserializedOptions = JsonSerializer.Deserialize<BlobStorageOptions>(serializedOptions);

        options.ConnectionString = deserializedOptions.ConnectionString;
        options.ContainerName = deserializedOptions.ContainerName;
    }
}
