using Refit;

namespace Xprtz.FlipIt.Contract.Cards;

public interface ICardApi
{
    [Get("/topic/{topicId}/card")]
    Task<IApiResponse<IReadOnlyCollection<Card>>> GetAll(Guid topicId);

    [Get("/topic/{topicId}/card/{id}")]
    Task<IApiResponse<Card>> Get(Guid topicId, Guid id);

    [Post("/topic/{topicId}/card")]
    Task<IApiResponse<Card>> Create(Guid topicId, CardRequest request);

    [Put("/topic/{topicId}/card/{id}")]
    Task<IApiResponse<Card>> Update(Guid topicId, Guid id, CardRequest request);

    [Delete("/topic/{topicId}/card/{id}")]
    Task<IApiResponse> Delete(Guid topicId, Guid id);
}
