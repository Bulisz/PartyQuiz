using Domain.Entities.Base;

namespace Domain.Entities;

public sealed class Answer : BaseEntity
{
    public string AnswerText { get; private init; } = string.Empty;
    public bool IsCorrect { get; private init; }
    public Guid QuestionId { get; private init; }
    public Question Question { get; private init; } = null!;

    private Answer(Guid id, string answerText, bool isCorrect, Question question) : base(id)
    {
        AnswerText = answerText;
        IsCorrect = isCorrect;
        QuestionId = question.Id;
    }

    public static Answer Create(Guid id, string answerText, bool isCorrect, Question question)
    {
        var answer = new Answer(id, answerText, isCorrect, question);

        // ToDo validation

        return answer;
    }
}
