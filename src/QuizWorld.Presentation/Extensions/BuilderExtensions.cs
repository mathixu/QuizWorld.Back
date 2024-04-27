using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using QuizWorld.Application;
using QuizWorld.Infrastructure;
using QuizWorld.Presentation.OptionsSetup;
using System.Reflection;

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
        builder.Services.AddControllers();

        builder.ConfigureSwagger();
        builder.ConfigureOptions();

        builder.ConfigureCors();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddInfrastructureServices(builder.Configuration);

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
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });

        return builder;
    }
    }
