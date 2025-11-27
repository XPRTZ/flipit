using System.Diagnostics;

namespace Xprtz.FlipIt.Infrastructure.Telemetry;

public abstract class TelemetryBase
{
    public const string Source = "Xprtz.FlipIt.Infrastructure.Telemetry";
    
    protected static readonly ActivitySource RegisteredActivity = new (Source);
}
