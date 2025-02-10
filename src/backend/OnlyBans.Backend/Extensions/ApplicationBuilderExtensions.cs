namespace OnlyBans.Backend.Extensions;

public static class ApplicationBuilderExtensions {
    public static void ConfigureMiddleware(this WebApplication app) {
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger(c => c.RouteTemplate = "api/swagger/{documentName}/swagger.json");
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "OnlyBans"); });
            app.UseDeveloperExceptionPage();
            app.UseCors("DevCors");
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}
