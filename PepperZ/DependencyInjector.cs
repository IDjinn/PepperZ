using Microsoft.Extensions.DependencyInjection;

namespace PepperZ;

public static class DependencyInjector
{
    public static IServiceCollection AddPepperZ(this IServiceCollection services, Action<PepperZConfiguration> config)
    {
        services.AddSingleton<IPepperZ, PepperZ>();
        services.AddSingleton<PepperZConfiguration>(services =>
        {
            var pepperZConfig = new PepperZConfiguration();
            config.Invoke(pepperZConfig);
            return pepperZConfig;
        });
        return services;
    }
}