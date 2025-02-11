using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OnlyBans.Backend.Database;
using OnlyBans.Backend.Models.Users;

namespace OnlyBans.Backend.Extensions;

public static class IdentityExtensions {
    
    public static IServiceCollection AddApplicationIdentity(this IServiceCollection services) {
        
        services.AddIdentityCore<User>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "@abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-.";
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddRoles<UserRole>()
            .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User, UserRole>>()
            .AddSignInManager();

        services.AddScoped<IRoleStore<UserRole>, RoleStore<UserRole, AppDbContext, Guid>>();
        services.AddScoped<IUserStore<User>, UserStore<User, UserRole, AppDbContext, Guid>>();

        return services;
    }
}