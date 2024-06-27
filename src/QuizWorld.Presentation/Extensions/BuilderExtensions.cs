using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using MongoDB.Driver.Core.Configuration;
using QuizWorld.Application;
using QuizWorld.Infrastructure;
using QuizWorld.Presentation.OptionsSetup;
using QuizWorld.Presentation.WebSockets;
using System.Reflection;
using System.Text.Json.Serialization;
using Constants = QuizWorld.Application.Common.Helpers.Constants;

namespace QuizWorld.Presentation.Extensions;

/// <summary>
/// Static class used to extend the functionality of the WebApplicationBuilder.
/// </summary>
public static class BuilderExtensions
{

    /// <summary>
    /// Configures the WebApplicationBuilder.
    /// </summary>
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder.Services.AddSignalR();

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        builder.ConfigureSwagger();
        builder.ConfigureOptions();

        builder.Services.AddScoped<WebSocketService>();

        builder.ConfigureCors();

        builder.Services.AddHttpContextAccessor();

        builder.Configuration.AddAzureKeyVault(new Uri(Environment.GetEnvironmentVariable(Constants.ENV_VARIABLE_KEY_KEY_VAULT_URL)), new DefaultAzureCredential());

        //builder.ConfigureApplicationInsights();

        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddInfrastructureServices(builder.Configuration);

        builder.ConfigureAuthentication();

        return builder;
    }

    private static WebApplicationBuilder ConfigureApplicationInsights(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration["ApplicationInsights:ConnectionString"];

        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("Application Insights Instrumentation Key is missing");

        builder.Services.AddApplicationInsightsTelemetry(options =>
        {
            options.ConnectionString = connectionString;
        });

        return builder;
    }

    private static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(options =>
            {
                builder.Configuration.Bind("AzureAd", options);

                options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token validated");
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Authentication failed");
                        return Task.CompletedTask;
                    },
                };
            }, options => { builder.Configuration.Bind("AzureAd", options); });

        return builder;
    }

    private static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var files = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
            foreach (var file in files)
            {
                c.IncludeXmlComments(file);
            }

            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme,
                },
            };

            c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            c.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
        });

        return builder;
    }

    private static WebApplicationBuilder ConfigureOptions(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureOptions<MongoDbOptionsSetup>();
        builder.Services.ConfigureOptions<BlobStorageOptionsSetup>();
        builder.Services.ConfigureOptions<LLMOptionsSetup>();

        return builder;
    }

    private static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAll",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")  // Autoriser le domaine local d'Angular
                            .WithOrigins("https://kind-hill-036ca5803.5.azurestaticapps.net")
                            .WithOrigins("https://purple-sky-034637e03.5.azurestaticapps.net")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();

                });
        });

        return builder;
    }
}
