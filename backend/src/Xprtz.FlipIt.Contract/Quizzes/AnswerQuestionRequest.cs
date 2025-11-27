namespace Xprtz.FlipIt.Contract.Quizzes;

public class AnswerQuestionRequest
{
    public Guid QuestionId { get; init; }
    public bool IsCorrect { get; init; }
}
