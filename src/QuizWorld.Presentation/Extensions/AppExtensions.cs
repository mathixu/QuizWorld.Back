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
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
