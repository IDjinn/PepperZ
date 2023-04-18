using Microsoft.Extensions.DependencyInjection;

namespace PepperZ.Tests;

public class UnitTest1
{
    private const string PEPPER = "REALLY-LONG-PEPPER-STRING-HERE";
    private readonly IPepperZ _pepperZ;

    public UnitTest1()
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
}