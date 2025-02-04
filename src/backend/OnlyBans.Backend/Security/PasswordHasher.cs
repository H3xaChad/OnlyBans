namespace OnlyBans.Backend.Security;

using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher {

    private const int DegreeOfParallelism = 4;
    private const int MemorySize = 65536;
    private const int Iterations = 3;
    private const char SaltDelimiter = '$';
    
    public static string HashPassword(string password) {
        var salt = RandomNumberGenerator.GetBytes(16);
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)) {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            MemorySize = MemorySize,
            Iterations = Iterations
        };
        return Convert.ToBase64String(argon2.GetBytes(32)) + SaltDelimiter + Convert.ToBase64String(salt);
    }

    public static bool VerifyPassword(string password, string storedHash) {
        var parts = storedHash.Split(SaltDelimiter);
        var hash = Convert.FromBase64String(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)) {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            MemorySize = MemorySize,
            Iterations = Iterations
        };
        return hash.SequenceEqual(argon2.GetBytes(32));
    }
}