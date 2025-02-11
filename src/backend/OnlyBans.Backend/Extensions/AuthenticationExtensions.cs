using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using OnlyBans.Backend.Config;

namespace OnlyBans.Backend.Extensions;
public static class AuthenticationExtensions {

    private const string PreferredProvider = "Bosch";

    private static OidcConfig GetProvider(IConfiguration config) {
        
        var providers = config.GetSection("OAuthProviders").Get<List<OidcConfig>>();
        if (providers is null || providers.Count == 0)
            throw new InvalidOperationException("No OAuth Providers found in configuration.");
        
        var json = JsonSerializer.Serialize(providers, new JsonSerializerOptions { WriteIndented = true });
        Debug.WriteLine("OAuth Providers configuration:");
        Debug.WriteLine(json);
        
        foreach (var p in providers) {
            var clientSecret = config[$"OAuthSecrets:{p.Name}:ClientSecret"];
            if (string.IsNullOrWhiteSpace(clientSecret))
                throw new InvalidOperationException($"No Client Secret found for {p.Name}!");
            p.ClientSecret = clientSecret;
        }
        
        var provider = providers.FirstOrDefault(p => p.Name == PreferredProvider);
        if (provider is null)
            throw new InvalidOperationException($"Selected OAuth Provider '{PreferredProvider}' not found in configuration.");

        return provider;
    }
    
    public static IServiceCollection AddOAuth2Authentication(this IServiceCollection services, IConfiguration config) {

        var provider = GetProvider(config);
        Console.WriteLine($"Provider: {provider.Name} with Secret: {provider.ClientSecret}");

        services.AddAuthentication()
            // .AddCookie(IdentityConstants.ApplicationScheme, opt => {
            //     opt.Cookie.Name = "Manager.Auth";
            //     opt.Cookie.IsEssential = true;
            //     opt.ExpireTimeSpan = TimeSpan.FromHours(8);
            //     opt.Events.OnRedirectToLogin = context => {
            //         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //         return Task.CompletedTask;
            //     };
            //     opt.Events.OnRedirectToAccessDenied = context => {
            //         context.Response.StatusCode = StatusCodes.Status403Forbidden;
            //         return Task.CompletedTask;
            //     };
            // })
            .AddOpenIdConnect("bosch", provider.Name, opt => {
                opt.ClientId = provider.ClientId;
                opt.ClientSecret = provider.ClientSecret;
                opt.MetadataAddress = provider.WellKnownUrl;
                opt.GetClaimsFromUserInfoEndpoint = true;
                opt.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;
                opt.SignInScheme = IdentityConstants.ExternalScheme;
                opt.CallbackPath = $"/api/v1/login/{provider.Name}";
                foreach (var scope in provider.Scopes
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())) {
                    opt.Scope.Add(scope);
                }
            });
            // .AddCookie(IdentityConstants.ExternalScheme, opt => {
            //     opt.Cookie.Name = "Manager.External";
            // });
        return services;
    }
}