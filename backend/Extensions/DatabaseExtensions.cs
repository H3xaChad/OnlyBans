using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlyBans.Backend.Database;

namespace OnlyBans.Backend.Extensions;

public static class DatabaseExtensions {
    
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) {
        var postgresConnection = configuration.GetConnectionString("Postgres") 
                                 ?? throw new Exception("Database connection string is missing");

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(postgresConnection));
        return services;
    }
}