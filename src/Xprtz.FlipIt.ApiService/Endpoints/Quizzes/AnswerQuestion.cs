using FastEndpoints;
using FluentValidation.Results;
using Xprtz.FlipIt.Application.Quizzes.Commands;
using Xprtz.FlipIt.Contract.Quizzes;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.ApiService.Endpoints.Quizzes;

public class AnswerQuestion(IRequestHandler<AnswerQuestionCommand, ErrorOr<Success>> handler)
    : Endpoint<AnswerQuestionRequest>
{
    public override void Configure()
    {
        Post("quiz/{id}/answer-question");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AnswerQuestionRequest request, CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var result = await handler.Handle(new(id, request.QuestionId, request.IsCorrect), ct);

        await result.Match(
            async _ => await SendNoContentAsync(ct),
            async x =>
            {
                ValidationFailures.AddRange(x.Select(e => new ValidationFailure(e.Code, e.Description)));
                await SendErrorsAsync(cancellation: ct);
            }
        );
    }
}
