using QuizWorld.Presentation.WebSockets;

namespace QuizWorld.Presentation.Extensions;

/// <summary>
/// Static class used to extend the functionality of the WebApplication.
/// </summary>
public static class AppExtensions
{
    /// <summary>
    /// Configures the WebApplication.
    /// </summary>
    public static WebApplication Configure(this WebApplication app)
    {
        // Configure the HTTP request pipeline. Env variable is "ASPNETCORE_ENVIRONMENT": "Development"
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowAll");

        app.MapHub<QuizSessionHub>("/quiz-session").RequireAuthorization();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
