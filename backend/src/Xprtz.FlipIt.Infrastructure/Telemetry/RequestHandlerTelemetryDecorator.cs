using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.Infrastructure.Telemetry;

public class RequestHandlerTelemetryDecorator<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
    : TelemetryBase, IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
    {
        using var activity = RegisteredActivity.StartActivity(typeof(TRequest).Name);
        
        return await handler.Handle(request, cancellationToken);
    }
}
