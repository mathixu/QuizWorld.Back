using Microsoft.Extensions.Options;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Application.Interfaces;
using QuizWorld.Infrastructure.Common.Options;
using System.Text.Json;

namespace QuizWorld.Presentation.OptionsSetup;

/// <summary>
/// The setup for the MongoDb options.
/// </summary>
public class MongoDbOptionsSetup(IKeyVaultService keyVaultService) : IConfigureOptions<MongoDbOptions>
{
    private readonly IKeyVaultService _keyVaultService = keyVaultService;

    public void Configure(MongoDbOptions options)
    {
        Task.Run(async () =>
        {
            var optionsJson = await _keyVaultService.GetSecretAsync(Constants.KEY_VAULT_SECRET_MONGO_DB_SETTINGS);

            var des = JsonSerializer.Deserialize<MongoDbOptions>(optionsJson);

            options.ConnectionString = des.ConnectionString;
            options.DatabaseName = des.DatabaseName;

        }).Wait();
    }
}
