using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xprtz.FlipIt.Domain.QuizAggregate;

namespace Xprtz.FlipIt.Infrastructure.Quizzes;

internal class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TopicName).IsRequired();
        builder.Property(x => x.ShowFirst).IsRequired();
        builder.Property(x => x.IsPracticeMode).IsRequired();
        builder
            .HasMany(x => x.Questions)
            .WithOne(x => x.Quiz)
            .HasForeignKey("QuizId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
