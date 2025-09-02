using Xprtz.FlipIt.Domain.CardAggregate;
using Xprtz.FlipIt.Domain.QuizAggregate;
using Xprtz.FlipIt.Domain.TopicAggregate;

namespace Xprtz.FlipIt.ApiService.Endpoints.Quizzes;

public static class QuizMapper
{
    public static IReadOnlyCollection<Contract.Quizzes.Quiz> ToContract(
        this IReadOnlyCollection<Quiz> quizzes
    ) => quizzes.Select(ToContract).ToList();

    public static Contract.Quizzes.Quiz ToContract(this Quiz quiz) =>
        new()
        {
            Id = quiz.Id,
            TopicName = quiz.TopicName,
            ShowFirst = quiz.ShowFirst.ToContract(),
            IsPracticeMode = quiz.IsPracticeMode,
            NumberOfQuestions = quiz.Questions.Count,
            NumberOfAnsweredQuestions = quiz.Questions.Count(q => q.IsAnswered),
            NumberOfCorrectAnswers = quiz.Questions.Count(q => q.IsCorrect)
        };

    public static Contract.Quizzes.Question ToContract(
        this Question question,
        Quiz quiz,
        Topic topic,
        Card card
    ) =>
        new()
        {
            Id = question.Id,
            Topic = topic.Name,
            ShowFirst = quiz.ShowFirst.ToContract(),
            FrontLabel = topic.FrontLabel,
            FrontText = card.Front,
            BackLabel = topic.BackLabel,
            BackText = card.Back
        };

    private static Contract.Quizzes.FrontOrBack ToContract(this FrontOrBack showFirst) =>
        showFirst switch
        {
            FrontOrBack.Front => Contract.Quizzes.FrontOrBack.Front,
            FrontOrBack.Back => Contract.Quizzes.FrontOrBack.Back,
            _ => throw new ArgumentOutOfRangeException(nameof(showFirst), showFirst, null)
        };

    public static FrontOrBack ToDomain(this Contract.Quizzes.FrontOrBack frontOrBack) =>
        frontOrBack switch
        {
            Contract.Quizzes.FrontOrBack.Front => FrontOrBack.Front,
            Contract.Quizzes.FrontOrBack.Back => FrontOrBack.Back,
            _ => throw new ArgumentOutOfRangeException(nameof(frontOrBack), frontOrBack, null)
        };
}
