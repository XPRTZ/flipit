using FastEndpoints;
using Xprtz.FlipIt.Application.Quizzes.Queries;
using Xprtz.FlipIt.Contract.Quizzes;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.ApiService.Endpoints.Quizzes;

public class GetById(IRequestHandler<GetQuizQuery, ErrorOr<Domain.QuizAggregate.Quiz>> handler)
    : EndpointWithoutRequest<Quiz>
{
    public override void Configure()
    {
        Get("quiz/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var result = await handler.Handle(new(id), ct);

        await result.Match(async x => await SendOkAsync(x.ToContract(), ct), async _ => await SendNotFoundAsync(ct));
    }
}
