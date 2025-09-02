using FastEndpoints;
using FluentValidation.Results;
using Xprtz.FlipIt.Application.Cards.Queries;
using Xprtz.FlipIt.Application.Quizzes.Commands;
using Xprtz.FlipIt.Contract.Quizzes;
using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.ApiService.Endpoints.Quizzes;

public class Create(
    IRequestHandler<GetAllCardsQuery, IReadOnlyCollection<Card>> getAllCardsQueryHandler,
    IRequestHandler<CreateQuizCommand, ErrorOr<Domain.QuizAggregate.Quiz>> createQuizCommandHandler
) : Endpoint<QuizRequest, Quiz>
{
    public override void Configure()
    {
        Post("quiz");
        AllowAnonymous();
    }

    public override async Task HandleAsync(QuizRequest request, CancellationToken ct)
    {
        var cards = await getAllCardsQueryHandler.Handle(new(request.TopicId), ct);

        var result = await createQuizCommandHandler.Handle(
            new(
                request.TopicName,
                request.NumberOfQuestions,
                request.ShowFirst.ToDomain(),
                request.IsPracticeMode,
                cards
            ),
            ct
        );

        await result.Match(
            async x => await SendCreatedAtAsync<GetById>(new { id = x.Id }, x.ToContract(), cancellation: ct),
            async x =>
            {
                ValidationFailures.AddRange(x.Select(e => new ValidationFailure(e.Code, e.Description)));
                await SendErrorsAsync(cancellation: ct);
            }
        );
    }
}
