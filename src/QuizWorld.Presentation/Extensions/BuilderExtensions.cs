using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuizWorld.Application;
using QuizWorld.Domain.Enums;
using QuizWorld.Infrastructure;
using QuizWorld.Infrastructure.Common.Options;
using QuizWorld.Presentation.OptionsSetup;
using System.Reflection;
using System.Text;

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
        builder.ConfigurePolicies();
        builder.ConfigureBearer();

        builder.ConfigureCors();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddInfrastructureServices(builder.Configuration);

        return builder;
    }

    private static WebApplicationBuilder ConfigureBearer(this WebApplicationBuilder builder)
    {
        var jwtOptions = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>().Value ?? throw new ArgumentNullException("JwtOptions");

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return builder;
    }

    private static WebApplicationBuilder ConfigurePolicies(this WebApplicationBuilder builder)
    {
        var ADMIN_ROLE = AvailableRoles.Admin;
        var TEACHER_ROLE = AvailableRoles.Teacher;
        var PLAYER_ROLE = AvailableRoles.Player;

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(ADMIN_ROLE, policy => policy.RequireRole(ADMIN_ROLE));

            options.AddPolicy(TEACHER_ROLE, policy => policy.RequireRole(TEACHER_ROLE, ADMIN_ROLE));

            options.AddPolicy(PLAYER_ROLE, policy => policy.RequireRole(PLAYER_ROLE, TEACHER_ROLE, ADMIN_ROLE));
        });

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
        builder.Services.ConfigureOptions<JwtOptionsSetup>();
        builder.Services.ConfigureOptions<RefreshTokenOptionsSetup>();

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
