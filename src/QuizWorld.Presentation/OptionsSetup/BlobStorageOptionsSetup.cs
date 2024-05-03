using Microsoft.Extensions.Options;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Interfaces;
using QuizWorld.Infrastructure.Common.Options;
using System.Text.Json;

namespace QuizWorld.Presentation.OptionsSetup;

public class BlobStorageOptionsSetup(IKeyVaultService keyVaultService) : IConfigureOptions<BlobStorageOptions>
{
    private readonly IKeyVaultService _keyVaultService = keyVaultService;

    public void Configure(BlobStorageOptions options)
    {
        Task.Run(async () =>
        {
            var optionsJson = await _keyVaultService.GetSecretAsync(Constants.KEY_VAULT_SECRET_BLOB_STORAGE_SETTINGS);

            var des = JsonSerializer.Deserialize<BlobStorageOptions>(optionsJson);

            options.ConnectionString = des.ConnectionString;
            options.ContainerName = des.ContainerName;
        }).Wait();
    }
}
