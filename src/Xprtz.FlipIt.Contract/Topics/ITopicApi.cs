using Refit;

namespace Xprtz.FlipIt.Contract.Topics;

public interface ITopicApi
{
    [Get("/topic")]
    Task<IApiResponse<IReadOnlyCollection<Topic>>> GetAll();

    [Get("/topic/{id}")]
    Task<IApiResponse<Topic>> Get(Guid id);

    [Post("/topic")]
    Task<IApiResponse<Topic>> Create(TopicRequest request);
    
    [Put("/topic/{id}")]
    Task<IApiResponse<Topic>> Update(Guid id, TopicRequest request);
    
    [Delete("/topic/{id}")]
    Task<IApiResponse> Delete(Guid id);
}
