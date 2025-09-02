using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xprtz.FlipIt.Domain.CardAggregate;

namespace Xprtz.FlipIt.Infrastructure.Cards;

internal class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TopicId).IsRequired();
        builder.Property(x => x.Front).IsRequired();
        builder.Property(x => x.Back).IsRequired();
        builder.Property(x => x.NumberOfViews).IsRequired();
        builder.Property(x => x.NumberOfCorrectAnswers).IsRequired();
        builder.Property(x => x.LastViewedAt).IsRequired(false);
    }
}
