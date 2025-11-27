using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xprtz.FlipIt.Domain.TopicAggregate;

namespace Xprtz.FlipIt.Infrastructure.Topics;

internal class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.FrontLabel).IsRequired();
        builder.Property(x => x.BackLabel).IsRequired();
    }
}
