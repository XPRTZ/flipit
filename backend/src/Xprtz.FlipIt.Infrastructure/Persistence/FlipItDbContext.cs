using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.QuizAggregate;
using Xprtz.FlipIt.Domain.SeedWork;
using Xprtz.FlipIt.Domain.SeedWork.DomainEvents;
using Xprtz.FlipIt.Domain.TopicAggregate;
using Xprtz.FlipIt.Infrastructure.Common.Middleware;

namespace Xprtz.FlipIt.Infrastructure.Persistence;

internal class FlipItDbContext(
    DbContextOptions options,
    IHttpContextAccessor? httpContextAccessor = null,
    IBus? messageBus = null
) : DbContext(options)
{
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Card> Cards => Set<Card>();
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<Question> Questions => Set<Question>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FlipItDbContext).Assembly);

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<AggregateRoot>()
            .SelectMany(entry => entry.Entity.PopDomainEvents())
            .ToList();

        if (IsUserWaitingOnline())
        {
            AddDomainEventsToOfflineProcessingQueue(domainEvents);
            return await base.SaveChangesAsync(cancellationToken);
        }

        await PublishDomainEvents(domainEvents);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private bool IsUserWaitingOnline() => httpContextAccessor?.HttpContext is not null;

    private async Task PublishDomainEvents(List<DomainEvent> domainEvents)
    {
        if (messageBus is null)
        {
            return;
        }

        foreach (var domainEvent in domainEvents)
        {
            await messageBus.Publish(domainEvent, domainEvent.GetType());
        }
    }

    private void AddDomainEventsToOfflineProcessingQueue(List<DomainEvent> domainEvents)
    {
        if (httpContextAccessor is null)
        {
            return;
        }

        var domainEventsQueue =
            httpContextAccessor.HttpContext!.Items.TryGetValue(
                EventualConsistencyMiddleware.DomainEventsKey,
                out var value
            ) && value is Queue<DomainEvent> existingDomainEvents
                ? existingDomainEvents
                : new();

        domainEvents.ForEach(domainEventsQueue.Enqueue);
        httpContextAccessor.HttpContext.Items[EventualConsistencyMiddleware.DomainEventsKey] = domainEventsQueue;
    }
}
