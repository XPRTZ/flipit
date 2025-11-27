using MassTransit;
using System.Diagnostics;

namespace Xprtz.FlipIt.Infrastructure.Telemetry;

public class ActivityTracingConsumeFilter : TelemetryBase, IFilter<ConsumeContext>
{
    public void Probe(ProbeContext context)
    {
    }

    public async Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
    {
        using var activity = StartActivity(context);
        await next.Send(context);
    }

    private static Activity StartActivity(ConsumeContext context)
    {
        var (traceParent, traceState) = GetTraceState(context);

        var activityContext = string.IsNullOrWhiteSpace(traceParent)
            ? new()
            : ActivityContext.Parse(traceParent, traceState);

        return RegisteredActivity.StartActivity(ActivityKind.Consumer,
            activityContext,
            [],
            [],
            DateTimeOffset.UtcNow,
            context.SupportedMessageTypes.FirstOrDefault() ?? context.GetType().Name)!;
    }

    private static (string? traceParent, string? traceState) GetTraceState(MessageContext context)
    {
        if (!context.Headers.TryGetHeader("traceParent", out var traceParent))
        {
            return (null, null);
        }

        if (!string.IsNullOrEmpty(traceParent.ToString()))
        {
            return context.Headers.TryGetHeader("traceState", out var traceState)
                ? (traceParent.ToString(), traceState.ToString())
                : (traceParent.ToString(), null);
        }

        return (null, null);
    }
}
