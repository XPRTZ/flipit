namespace Xprtz.FlipIt.Contract.Quizzes;

public class QuizRequest
{
    public Guid TopicId { get; init; }
    public required string TopicName { get; init; }
    public int? NumberOfQuestions { get; init; }
    public FrontOrBack ShowFirst { get; init; }
    public bool IsPracticeMode { get; init; }
}
