using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Xprtz.FlipIt.Domain.CardAggregate.Rules;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCqrs();
        services.AddRules();

        return services;
    }

    private static void AddCqrs(this IServiceCollection services)
    {
        var typesToRegister = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(x =>
                !x.IsAbstract
                && x.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
            )
            .ToList();

        foreach (var type in typesToRegister)
        {
            var interfaceType = type.GetInterfaces()
                .First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

            var descriptor = new ServiceDescriptor(interfaceType, type, ServiceLifetime.Scoped);
            services.Add(descriptor);
        }
    }

    private static void AddRules(this IServiceCollection services)
    {
        var ruleType = typeof(IIncludeInQuizRule);
        var typesToRegister = Assembly
            .GetAssembly(ruleType)!
            .GetTypes()
            .Where(x => ruleType.IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false })
            .ToList();

        foreach (var type in typesToRegister)
        {
            var descriptor = new ServiceDescriptor(ruleType, type, ServiceLifetime.Scoped);
            services.Add(descriptor);
        }
    }
}
