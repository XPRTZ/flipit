using FastEndpoints;
using Xprtz.FlipIt.Application.Cards.Queries;
using Xprtz.FlipIt.Application.Quizzes.Queries;
using Xprtz.FlipIt.Application.Topics.Queries;
using Xprtz.FlipIt.Contract.Quizzes;
using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;
using Xprtz.FlipIt.Domain.TopicAggregate;

namespace Xprtz.FlipIt.ApiService.Endpoints.Quizzes;

public class GetNextQuestion(
    IRequestHandler<GetNextQuestionQuery, ErrorOr<Domain.QuizAggregate.Question>> getNextQuestionQueryHandler,
    IRequestHandler<GetQuizQuery, ErrorOr<Domain.QuizAggregate.Quiz>> getQuizQueryHandler,
    IRequestHandler<GetCardQuery, ErrorOr<Card>> getCardQueryHandler,
    IRequestHandler<GetTopicQuery, ErrorOr<Topic>> getTopicQueryHandler
) : EndpointWithoutRequest<Question>
{
    public override void Configure()
    {
        Get("quiz/{id}/next-question");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var result = await getNextQuestionQueryHandler.Handle(new(id), ct);

        await result.Match(
            async x =>
            {
                var question = await MapToContract(x, id, ct);
                if (question.IsError)
                {
                    await SendNotFoundAsync(ct);
                }
                else
                {
                    await SendOkAsync(question.Value, ct);
                }
            },
            async _ => await SendNotFoundAsync(ct)
        );
    }

    private async Task<ErrorOr<Question>> MapToContract(
        Domain.QuizAggregate.Question question,
        Guid quizId,
        CancellationToken ct
    )
    {
        var quiz = await getQuizQueryHandler.Handle(new(quizId), ct);
        if (quiz.IsError)
        {
            return Error.NotFound();
        }

        var card = await getCardQueryHandler.Handle(new(question.CardId), ct);
        if (card.IsError)
        {
            return Error.NotFound();
        }

        var topic = await getTopicQueryHandler.Handle(new(card.Value.TopicId), ct);
        if (topic.IsError)
        {
            return Error.NotFound();
        }

        return question.ToContract(quiz.Value, topic.Value, card.Value);
    }
}
