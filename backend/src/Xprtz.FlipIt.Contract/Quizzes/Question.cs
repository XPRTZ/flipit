namespace Xprtz.FlipIt.Contract.Quizzes;

public class Question
{
    public Guid Id { get; init; }
    public required string Topic { get; init; }
    public FrontOrBack ShowFirst { get; init; }
    public required string FrontLabel { get; init; }
    public required string FrontText { get; init; }
    public required string BackLabel { get; init; }
    public required string BackText { get; init; }
}
