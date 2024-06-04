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
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IUserSessionRepository, UserSessionRepository>();
        services.AddScoped<IUserResponseRepository, UserResponseRepository>();
        services.AddScoped<IGeneratedContentRepository, GeneratedContentRepository>();

        services.AddScoped<IQuestionGenerator, QuestionGenerator>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddSingleton<IStorageService, BlobStorageService>();
    
        services.AddScoped<ILLMService, OpenAIChatCompletion>();

        return services;
    }
}