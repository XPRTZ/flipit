using Refit;

namespace Xprtz.FlipIt.Contract.Quizzes;

public interface IQuizApi
{
    [Get("/quiz")]
    Task<IApiResponse<IReadOnlyCollection<Quiz>>> GetAll();

    [Get("/quiz/{id}")]
    Task<IApiResponse<Quiz>> Get(Guid id);

    [Post("/quiz")]
    Task<IApiResponse<Quiz>> Create(QuizRequest request);

    [Get("/quiz/{id}/next-question")]
    Task<IApiResponse<Question>> GetNextQuestion(Guid id);

    [Post("/quiz/{id}/answer-question")]
    Task<IApiResponse> AnswerQuestion(Guid id, AnswerQuestionRequest request);

    [Delete("/quiz/{id}")]
    Task<IApiResponse> Delete(Guid id);
}
