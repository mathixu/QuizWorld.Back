using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
using QuizWorld.Infrastructure.Interfaces;
using QuizWorld.Infrastructure.Persistence.Repositories;
using QuizWorld.Infrastructure.Services;

namespace QuizWorld.Infrastructure;

public static class ConfigureServices
{
    /// <summary>
    /// Adds the infrastructure services to the service collection.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        services.AddScoped<IIdentityService, IdentityService>();
        
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IRefreshTokenProvider, RefreshTokenProvider>();

        services.AddSingleton<IHashService, HashService>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}