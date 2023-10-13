using Domain.Entities.Base;

namespace Domain.Entities;

public sealed class Question : BaseEntity
{
    public int QuestionNumber { get; private init; }
    public int FullScore { get; private set; }
    public string QuestionText { get; private set; } = string.Empty;
    public Guid RoundId { get; private init; }
    public Round Round { get; private init; } = null!;
    public ICollection<Answer> Answers { get; private init; } = null!;

    private Question(int questionNumber, int fullScore, string questionText, Guid roundId)
    {
        QuestionNumber = questionNumber;
        FullScore = fullScore;
        QuestionText = questionText;
        RoundId = roundId;
        Answers = new List<Answer>();
    }

    public static Question Create(int questionNumber, int fullScore, string questionText, Guid roundId)
    {
        var Question = new Question(questionNumber, fullScore, questionText, roundId);

        return Question;
    }

    public void Modify(int fullScore, string questionText)
    {
        FullScore = fullScore;
        QuestionText = questionText;
    }
}
