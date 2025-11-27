using MassTransit;
using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.Common;
using Xprtz.FlipIt.Domain.QuizAggregate.Events;
using Xprtz.FlipIt.Domain.SeedWork;

namespace Xprtz.FlipIt.Application.Cards.Events;

public class UpdateCardWhenQuestionAnswered(IUnitOfWork<Card> unitOfWork, IDateTimeProvider dateTimeProvider)
    : IConsumer<QuestionAnswered>
{
    public async Task Consume(ConsumeContext<QuestionAnswered> context)
    {
        var message = context.Message;

        var card =
            await unitOfWork.Repository.Get(message.CardId) ?? throw new InvalidOperationException("Card not found");

        card.Answered(message.IsCorrect, dateTimeProvider.Now);

        await unitOfWork.SaveChanges();
    }
}
