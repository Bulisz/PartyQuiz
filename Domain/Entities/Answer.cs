using Domain.Entities.Base;

namespace Domain.Entities;

public sealed class Answer : BaseEntity
{
    public string AnswerText { get; private init; } = string.Empty;
    public bool IsCorrect { get; private init; }
    public Guid QuestionId { get; private init; }
    public Question Question { get; private set; } = null!;

    private Answer(string answerText, bool isCorrect, Guid questionId)
    {
        AnswerText = answerText;
        IsCorrect = isCorrect;
        QuestionId = questionId;
    }

    public static Answer Create(string answerText, bool isCorrect, Guid questionId)
    {
        var answer = new Answer(answerText, isCorrect, questionId);

        // ToDo validation

        return answer;
    }
}
