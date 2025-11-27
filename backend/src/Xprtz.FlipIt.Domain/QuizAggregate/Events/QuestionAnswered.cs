using Xprtz.FlipIt.Domain.SeedWork.DomainEvents;

namespace Xprtz.FlipIt.Domain.QuizAggregate.Events;

public record QuestionAnswered(Guid CardId, bool IsCorrect) : DomainEvent;
