using Microsoft.Extensions.Options;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Presentation.OptionsSetup;

/// <summary>
/// The setup for the MongoDb options.
/// </summary>
public class MongoDbOptionSetup(IConfiguration configuration) : IConfigureOptions<MongoDbOptions>
{
    private const string SectionName = "MongoDb";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(MongoDbOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}
