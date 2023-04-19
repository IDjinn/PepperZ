using Microsoft.Extensions.DependencyInjection;

namespace PepperZ.Tests;

public class PasswordTests
{
    private const string PEPPER = "REALLY-LONG-PEPPER-STRING-HERE";
    private readonly IPepperZ _pepperZ;

    public PasswordTests()
    {
        var provider = new ServiceCollection()
            .AddOptions()
            .AddPepperZ(config => config.Pepper = PEPPER)
            .BuildServiceProvider();
        _pepperZ = provider.GetService<IPepperZ>()!;
    }

    [Fact]
    public void hash_and_check_if_its_equals()
    {
        const string rawPassowrd = "pepper-z-supersecure-pass0rd";
        var safePassword = _pepperZ.HashSaltAndPepperPassword(rawPassowrd);
        var result = _pepperZ.RawPasswordMatchCheck(safePassword, rawPassowrd);
        Assert.True(result);
        Assert.NotNull(safePassword.Salt);
        Assert.NotNull(safePassword.Value);
    }


    [Theory]
    [InlineData("$2a$12$uDC3C1/qgEO08kJ4t1uDkurV8Ti6vXDw5WVm1SDzKPZQmJ1HMMgdm", "$2a$12$uDC3C1/qgEO08kJ4t1uDku",
        "pepper-z-supersecure-pass0rd")]
    public void check_compiled_hashed_password(string hash, string salt, string rawPassowrd)
    {
        var safePassword = new Password(hash, salt);
        var result = _pepperZ.RawPasswordMatchCheck(safePassword, rawPassowrd);
        Assert.True(result);
    }
}