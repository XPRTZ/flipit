using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Sinks.OpenTelemetry;
using System.Reflection;
using Xprtz.FlipIt.Application.Quizzes.Commands;
using Xprtz.FlipIt.Application.Topics.Queries;
using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.Common;
using Xprtz.FlipIt.Domain.QuizAggregate;
using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;
using Xprtz.FlipIt.Domain.TopicAggregate;
using Xprtz.FlipIt.Infrastructure.Cards;
using Xprtz.FlipIt.Infrastructure.Common;
using Xprtz.FlipIt.Infrastructure.Common.Middleware;
using Xprtz.FlipIt.Infrastructure.Persistence;
using Xprtz.FlipIt.Infrastructure.Quizzes;
using Xprtz.FlipIt.Infrastructure.Telemetry;
using Xprtz.FlipIt.Infrastructure.Topics;

namespace Xprtz.FlipIt.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FlipItDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("FlipIt"))
        );

        services.AddMonitoring();

        services.AddMassTransit(x =>
        {
            x.AddConsumers(Assembly.GetAssembly(typeof(Application.DependencyInjection)));

            x.UsingInMemory((context, config) =>
                {
                    config.ConfigurePublish(configurator =>
                        configurator.AddPipeSpecification(new ActivityTracingPipeSpecification()));
                    config.ConfigureSend(configurator =>
                        configurator.AddPipeSpecification(new ActivityTracingPipeSpecification()));
                    config.AddPipeSpecification(new ActivityTracingPipeSpecification());
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

    private static void AddMonitoring(this IServiceCollection services)
    {
        // TODO: generiek maken
        services
            .Decorate<IRequestHandler<GetAllTopicsQuery, IReadOnlyCollection<Topic>>,
                RequestHandlerTelemetryDecorator<GetAllTopicsQuery, IReadOnlyCollection<Topic>>>();
        services
            .Decorate<IRequestHandler<AnswerQuestionCommand, ErrorOr<Success>>,
                RequestHandlerTelemetryDecorator<AnswerQuestionCommand, ErrorOr<Success>>>();

        services.AddLogging();

        services.AddOpenTelemetry()
            .WithMetrics(builder =>
            {
                builder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(builder =>
            {
                builder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddSource(TelemetryBase.Source);
            })
            .ConfigureResource(builder =>
            {
                builder.AddService("Xprtz.FlipIt.ApiService");
            })
            .UseOtlpExporter(OtlpExportProtocol.Grpc, new("http://localhost:4317"));
    }

    private static void AddLogging(this IServiceCollection services)
    {
        var loggerConfiguration = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
            .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Information)
            .WriteTo.Logger(configuration =>
            {
                configuration
                    .WriteTo.Console(
                        outputTemplate:
                        "[{Timestamp:HH:mm:ss} {Level:u3}]({SourceContext}) {Message:lj}{NewLine}{Exception}");
            })
            .WriteTo.Logger(configuration =>
            {
                configuration.WriteTo.OpenTelemetry(x =>
                {
                    x.Endpoint = "http://localhost:4317";
                    x.Protocol = OtlpProtocol.Grpc;
                    x.ResourceAttributes = new Dictionary<string, object>
                    {
                        ["service.name"] = "Xprtz.FlipIt.ApiService"
                    };
                });
            });

        var logger = loggerConfiguration.CreateLogger();

#if DEBUG
        Serilog.Debugging.SelfLog.Enable(Console.Error);
#endif

        services.AddLogging(x =>
        {
            x.ClearProviders();
            x.AddSerilog(logger, dispose: true);
        });
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
