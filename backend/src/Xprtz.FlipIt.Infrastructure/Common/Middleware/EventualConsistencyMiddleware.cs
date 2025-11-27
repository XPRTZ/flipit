using MassTransit;
using Microsoft.AspNetCore.Http;
using Xprtz.FlipIt.Domain.SeedWork.DomainEvents;
using Xprtz.FlipIt.Infrastructure.Persistence;

namespace Xprtz.FlipIt.Infrastructure.Common.Middleware;

internal class EventualConsistencyMiddleware(RequestDelegate next)
{
    public const string DomainEventsKey = "DomainEventsKey";

    public async Task InvokeAsync(HttpContext context, IBus messageBus, FlipItDbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();
        context.Response.OnCompleted(async () =>
        {
            try
            {
                if (
                    context.Items.TryGetValue(DomainEventsKey, out var value)
                    && value is Queue<DomainEvent> domainEvents
                )
                {
                    while (domainEvents.TryDequeue(out var nextEvent))
                    {
                        await messageBus.Publish(nextEvent, nextEvent.GetType());
                    }
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.DisposeAsync();
                throw;
            }
        });

        await next(context);
    }
}
