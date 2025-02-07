using System.Diagnostics;

namespace OnlyBans.Backend.Config;

public class PostgresConfig {

    public string Host { get; private set; } = "localhost";
    public int Port { get; private set; } = 5432;
    public string Database { get; private set; } = "postgres";
    public string Username { get; private set; } = "postgres";
    public string Password { get; private set; } = string.Empty;

    public string GetConnectionString() {
        return $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password}";
    }

    public void OverrideWithEnv(string prefix) {
        var env = Environment.GetEnvironmentVariables();
        Debug.WriteLine($"Read env vars: {env}");
        Host = env[$"{prefix}_HOST"]?.ToString() ?? Host;
        Port = int.TryParse(env[$"{prefix}_PORT"]?.ToString(), out var port) ? port : Port;
        Database = env[$"{prefix}_DATABASE"]?.ToString() ?? Database;
        Username = env[$"{prefix}_USERNAME"]?.ToString() ?? Username;
        Password = env[$"{prefix}_PASSWORD"]?.ToString() ?? Password;
    }
}