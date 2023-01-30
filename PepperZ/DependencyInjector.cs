using Microsoft.Extensions.DependencyInjection;

namespace PepperZ;

public static class DependencyInjector
{
    public static IServiceCollection AddPepperZ(this IServiceCollection services)
    {
        services.AddSingleton<IPepperZ, Core.PepperZ>();
        return services;
    }
}