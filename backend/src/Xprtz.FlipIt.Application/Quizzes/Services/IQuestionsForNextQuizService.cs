using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.QuizAggregate;

namespace Xprtz.FlipIt.Application.Quizzes.Services;

public interface IQuestionsForNextQuizService
{
    IReadOnlyCollection<Question> CreateQuestions(IReadOnlyCollection<Card> cards, int? numberOfQuestions);
}
