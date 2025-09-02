using FastEndpoints;
using Xprtz.FlipIt.Application.Quizzes.Commands;
using Xprtz.FlipIt.Domain.SeedWork.Cqrs;

namespace Xprtz.FlipIt.ApiService.Endpoints.Quizzes;

public class Delete(IRequestHandler<DeleteQuizCommand, ErrorOr<Success>> handler) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("quiz/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var result = await handler.Handle(new(id), ct);

        await result.Match(async _ => await SendNoContentAsync(ct), async _ => await SendNotFoundAsync(ct));
    }
}
