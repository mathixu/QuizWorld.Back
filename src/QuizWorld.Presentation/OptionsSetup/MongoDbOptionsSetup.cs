using Microsoft.Extensions.Options;
using QuizWorld.Application.Common.Helpers;
using QuizWorld.Infrastructure.Common.Options;
using System.Text.Json;

namespace QuizWorld.Presentation.OptionsSetup;

/// <summary>
/// The setup for the MongoDb options.
/// </summary>
public class MongoDbOptionsSetup(IConfiguration configuration) : IConfigureOptions<MongoDbOptions>
{
    private readonly IConfiguration _configuration = configuration;

    public void Configure(MongoDbOptions options)
    {
        var serializedOptions = _configuration[Constants.KEY_VAULT_SECRET_MONGO_DB_SETTINGS];

        var deserializedOptions = JsonSerializer.Deserialize<MongoDbOptions>(serializedOptions);

        options.ConnectionString = deserializedOptions.ConnectionString;
        options.DatabaseName = deserializedOptions.DatabaseName;
    }
}
