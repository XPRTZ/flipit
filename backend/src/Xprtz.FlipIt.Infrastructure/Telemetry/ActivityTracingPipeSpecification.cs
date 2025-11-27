using MassTransit;
using MassTransit.Configuration;

namespace Xprtz.FlipIt.Infrastructure.Telemetry;

public class ActivityTracingPipeSpecification : IPipeSpecification<PublishContext>, IPipeSpecification<SendContext>, IPipeSpecification<ConsumeContext>
{
    public IEnumerable<ValidationResult> Validate() => [];

    public void Apply(IPipeBuilder<PublishContext> builder) => builder.AddFilter(new ActivityTracingProduceFilter());

    public void Apply(IPipeBuilder<SendContext> builder) => builder.AddFilter(new ActivityTracingProduceFilter());
    
    public void Apply(IPipeBuilder<ConsumeContext> builder) => builder.AddFilter(new ActivityTracingConsumeFilter());
}
