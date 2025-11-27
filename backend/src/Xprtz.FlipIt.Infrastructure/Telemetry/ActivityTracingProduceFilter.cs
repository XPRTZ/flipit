using MassTransit;
using System.Diagnostics;

namespace Xprtz.FlipIt.Infrastructure.Telemetry;

public class ActivityTracingProduceFilter : TelemetryBase, IFilter<PublishContext>, IFilter<SendContext>
{
    public void Probe(ProbeContext context)
    {
    }

    public async Task Send(SendContext context, IPipe<SendContext> next)
    {
        using var activity = GetOrStartActivity();

        InjectHeaders(activity, context);

        await next.Send(context);
    }

    public async Task Send(PublishContext context, IPipe<PublishContext> next)
    {
        using var activity = GetOrStartActivity();

        InjectHeaders(activity, context);

        await next.Send(context);
    }

    private static Activity GetOrStartActivity() =>
        Activity.Current ?? RegisteredActivity.StartActivity(nameof(ActivityTracingProduceFilter))!;

    private static void InjectHeaders(
        Activity activity,
        SendContext context)
    {
        if (activity.IdFormat != ActivityIdFormat.W3C)
        {
            return;
        }

        if (context.Headers.TryGetHeader("traceParent", out _))
        {
            return;
        }

        context.Headers.Set("traceParent", activity.ParentId ?? activity.Id);
        if (activity.TraceStateString != null)
        {
            context.Headers.Set("traceState", activity.TraceStateString);
        }
    }
}
