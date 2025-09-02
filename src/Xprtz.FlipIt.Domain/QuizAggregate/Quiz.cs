using ErrorOr;
using Xprtz.FlipIt.Domain.QuizAggregate.Events;
using Xprtz.FlipIt.Domain.SeedWork;

namespace Xprtz.FlipIt.Domain.QuizAggregate;

public class Quiz : AggregateRoot
{
    public string TopicName { get; private set; }
    public FrontOrBack ShowFirst { get; private set; }
    public bool IsPracticeMode { get; private set; }
    public IReadOnlyCollection<Question> Questions { get; private set; } = new List<Question>();

    private Quiz(Guid id, string topicName, FrontOrBack showFirst, bool isPracticeMode)
        : base(id)
    {
        TopicName = topicName;
        ShowFirst = showFirst;
        IsPracticeMode = isPracticeMode;
    }

    private ErrorOr<Success> SetQuestions(IReadOnlyCollection<Question> questions)
    {
        if (questions.Count == 0)
        {
            return Error.Validation("Questions are required");
        }

        Questions = questions;

        return Result.Success;
    }

    public ErrorOr<Success> AnswerQuestion(Guid questionId, bool isCorrect)
    {
        var existingQuestion = Questions.FirstOrDefault(q => q.Id == questionId);

        if (existingQuestion is null)
        {
            return Error.Validation("Question not found");
        }

        var result = existingQuestion.Answer(isCorrect);

        if (IsPracticeMode || result.IsError)
        {
            return result;
        }

        RaiseDomainEvent(new QuestionAnswered(existingQuestion.CardId, isCorrect));

        return result;
    }

    public ErrorOr<Question> GetNextQuestion()
    {
        var nextQuestion = Questions.OrderBy(x => x.Order).FirstOrDefault(q => !q.IsAnswered);

        return nextQuestion == null ? Error.Validation("No more questions") : nextQuestion;
    }

    public static ErrorOr<Quiz> Create(
        string topicName,
        FrontOrBack showFirst,
        bool isPracticeMode,
        IReadOnlyCollection<Question> questions
    )
    {
        var quiz = new Quiz(Guid.NewGuid(), topicName, showFirst, isPracticeMode);
        var result = quiz.SetQuestions(questions);

        return result.IsError ? result.Errors : quiz;
    }

    public ErrorOr<Success> Delete()
    {
        // nothing for now, raise domain event later
        return Result.Success;
    }
}
