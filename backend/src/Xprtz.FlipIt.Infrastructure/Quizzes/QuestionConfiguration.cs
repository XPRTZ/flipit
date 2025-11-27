using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xprtz.FlipIt.Domain.QuizAggregate;

namespace Xprtz.FlipIt.Infrastructure.Quizzes;

internal class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CardId).IsRequired();
        builder.Property(x => x.Order).IsRequired();
        builder.Property(x => x.IsAnswered).IsRequired();
        builder.Property(x => x.IsCorrect).IsRequired();
        builder
            .HasOne(x => x.Quiz)
            .WithMany(x => x.Questions)
            .HasForeignKey("QuizId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
