// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace OnlyBans.Backend.Extensions {
//     public static class AuthenticationExtensions {
//         public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services, IConfiguration configuration) {
//             services.AddAuthentication(options => {
//                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//             })
//             .AddJwtBearer(options => {
//                 options.Authority = configuration["Authentication:Authority"];
//                 options.Audience = configuration["Authentication:Audience"];
//                 options.RequireHttpsMetadata = true;
//             });
//
//             return services;
//         }
//     }
// }
