using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizWorld.Application.Interfaces;
using QuizWorld.Application.Interfaces.Repositories;
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
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddSingleton<IStorageService, BlobStorageService>();

        return services;
    }
}