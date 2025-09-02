using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.Common;
using Xprtz.FlipIt.Domain.QuizAggregate;
using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.TopicAggregate;
using Xprtz.FlipIt.Infrastructure.Cards;
using Xprtz.FlipIt.Infrastructure.Common;
using Xprtz.FlipIt.Infrastructure.Common.Middleware;
using Xprtz.FlipIt.Infrastructure.Persistence;
using Xprtz.FlipIt.Infrastructure.Quizzes;
using Xprtz.FlipIt.Infrastructure.Topics;

namespace Xprtz.FlipIt.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FlipItDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("FlipIt"))
        );

        services.AddMassTransit(x =>
        {
            x.AddConsumers(Assembly.GetAssembly(typeof(Application.DependencyInjection)));

            x.UsingInMemory(
                (context, config) =>
                {
                    config.ConfigureEndpoints(context);
                }
            );
        });

        services.AddScoped<IUnitOfWork<Topic>, TopicUnitOfWork>();
        services.AddScoped<IUnitOfWork<Card>, CardUnitOfWork>();
        services.AddScoped<IUnitOfWork<Quiz>, QuizUnitOfWork>();

        services.AddScoped<IRandomNumberGenerator>(_ => new RandomNumberGenerator((int)DateTime.Now.Ticks));
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseMiddleware<EventualConsistencyMiddleware>();

        using var scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<FlipItDbContext>();
        context.Database.Migrate();

        return app;
    }
}
