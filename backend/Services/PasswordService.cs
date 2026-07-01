using System.Security.Cryptography;

namespace Backend.Api.Services;

public class PasswordService
{
    public string GenerateSalt()
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(16);
        return Convert.ToBase64String(saltBytes);
    }

    public string HashPassword(string password, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);

        byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltBytes,
            100_000,
            HashAlgorithmName.SHA256,
            32
        );

        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string password, string salt, string storedHash)
    {
        string computedHash = HashPassword(password, salt);
        return computedHash == storedHash;
    }
}
