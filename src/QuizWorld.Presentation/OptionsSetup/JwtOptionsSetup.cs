using Microsoft.Extensions.Options;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Presentation.OptionsSetup;

/// <summary>
/// The setup for the JWT options.
/// </summary>
public class JwtOptionsSetup(IConfiguration configuration) : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "Jwt";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(JwtOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}
