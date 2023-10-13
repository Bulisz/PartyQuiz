using Domain.Entities.Base;

namespace Domain.Entities;

public sealed class Answer : BaseEntity
{
    public string AnswerText { get; private set; } = string.Empty;
    public bool IsCorrect { get; private set; }
    public Guid QuestionId { get; private init; }
    public Question Question { get; private init; } = null!;

    private Answer(string answerText, bool isCorrect, Guid questionId)
    {
        AnswerText = answerText;
        IsCorrect = isCorrect;
        QuestionId = questionId;
    }

    public static Answer Create(string answerText, bool isCorrect, Guid questionId)
    {
        var answer = new Answer(answerText, isCorrect, questionId);

        return answer;
    }

    public void Modify(string answerText, bool isCorrect)
    {
        AnswerText = answerText;
        IsCorrect = isCorrect;
    }
}
