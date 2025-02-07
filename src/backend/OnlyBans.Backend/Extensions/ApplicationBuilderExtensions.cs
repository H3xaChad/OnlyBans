namespace OnlyBans.Backend.Extensions;

public static class ApplicationBuilderExtensions {
    public static void ConfigureMiddleware(this WebApplication app) {
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}
