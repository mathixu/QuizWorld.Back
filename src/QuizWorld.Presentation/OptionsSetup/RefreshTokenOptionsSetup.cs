using Microsoft.Extensions.Options;
using QuizWorld.Infrastructure.Common.Options;

namespace QuizWorld.Presentation.OptionsSetup;

/// <summary>
/// Represents the refresh token options setup.
/// </summary>
public class RefreshTokenOptionsSetup(IConfiguration configuration) : IConfigureOptions<RefreshTokenOptions>
{
    private const string SectionName = "RefreshToken";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(RefreshTokenOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}
