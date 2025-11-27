using Microsoft.Extensions.DependencyInjection;

namespace Xprtz.FlipIt.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCqrs();
        services.AddRules();

        return services;
    }
}
