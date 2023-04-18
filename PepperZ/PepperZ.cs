using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using BCryptor = BCrypt.Net.BCrypt;

namespace PepperZ;

internal sealed class PepperZ : IPepperZ
{
    private PepperZConfiguration _configuration;

    public PepperZ(PepperZConfiguration configuration)
    {
        _configuration = configuration;
        ArgumentException.ThrowIfNullOrEmpty(_configuration.Pepper);
    }

    public Password HashSaltAndPepperPassword(string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password);
        var safePassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        var encryptedWithPepperPassword =
            Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(safePassword + _configuration.Pepper)));
        var (salt, hash) = HashAndSalt(encryptedWithPepperPassword);
        return new Password(Value: hash, Salt: salt);
    }

    public bool RawPasswordMatchCheck(Password password, string rawPassword)
    {
        ArgumentException.ThrowIfNullOrEmpty(rawPassword);
        var safeRawPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(rawPassword));
        var encryptedRawSafeWithPepperPassword =
            Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(safeRawPassword + _configuration.Pepper)));
        var (_, hashedRawPassword) = HashAndSalt(encryptedRawSafeWithPepperPassword, password.Salt);
        return safe_string_equals(hashedRawPassword, password.Value);
    }

    private HashedSaltedPassword HashAndSalt(string value, string? salt = null)
    {
        salt ??= BCryptor.GenerateSalt(_configuration.WorkFactor);
        var hash = BCryptor.HashPassword(value, salt);
        return new(salt, hash);
    }

    private byte[] Encrypt(byte[] value)
    {
        using var sha = SHA384.Create();
        return sha.ComputeHash(value);
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private static bool safe_string_equals(string left, string right)
    {
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(left),
            Encoding.UTF8.GetBytes(right)
        );
    }

    private readonly record struct HashedSaltedPassword(string Salt, string Hash);
}