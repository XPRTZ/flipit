using FastEndpoints;
using Xprtz.FlipIt.Application.Quizzes.Queries;
using Xprtz.FlipIt.Contract.Quizzes;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.ApiService.Endpoints.Quizzes;

public class Get(IRequestHandler<GetAllQuizzesQuery, IReadOnlyCollection<Domain.QuizAggregate.Quiz>> handler)
    : EndpointWithoutRequest<IReadOnlyCollection<Quiz>>
{
    public override void Configure()
    {
        Get("quiz");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await handler.Handle(new(), ct);

        await SendOkAsync(result.ToContract(), ct);
    }
}
