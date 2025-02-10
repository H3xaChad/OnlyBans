namespace OnlyBans.Backend.Config;

public class OidcConfig {
    public required string Name { get; set; }
    public required string TenantId { get; set; }
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required string WellKnownUrl { get; set; }
    public required string Scopes { get; set; }
}