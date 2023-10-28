using CSharpFunctionalExtensions;

namespace Domain.Games;

public class Answer : Entity<Guid>
{
    public string AnswerText { get; private set; } = string.Empty;
    public bool IsCorrect { get; private init; }
    public Guid QuestionId { get; private init; }
    public Question Question { get; private init; } = null!;

    private Answer(string answerText, bool isCorrect, Guid questionId)
    {
        AnswerText = answerText;
        IsCorrect = isCorrect;
        QuestionId = questionId;
    }

    public static Result<Answer> Create(string answerText, bool isCorrect, Guid questionId)
    {
        var answer = new Answer(answerText, isCorrect, questionId);

        return answer;
    }

    public Result Modify(string answerText)
    {
        AnswerText = answerText;

        return Result.Success();
    }
}