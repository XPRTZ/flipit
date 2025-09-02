namespace Xprtz.FlipIt.Domain.SeedWork.Cqrs;

public interface IRequest;

public interface IRequest<out TResponse> : IRequest;
