namespace Xprtz.FlipIt.Contract.Quizzes;

public class Quiz
{
    public Guid Id { get; init; }
    public required string TopicName { get; init; }
    public FrontOrBack ShowFirst { get; init; }
    public bool IsPracticeMode { get; init; }
    public int NumberOfQuestions { get; init; }
    public int NumberOfAnsweredQuestions { get; init; }
    public int NumberOfCorrectAnswers { get; init; }

    public bool IsCompleted => NumberOfQuestions == NumberOfAnsweredQuestions;
}
