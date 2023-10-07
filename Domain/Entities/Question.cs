using Domain.Entities.Base;

namespace Domain.Entities;

public sealed class Question : BaseEntity
{
    public int QuestionNumber { get; private init; }
    public int FullScore { get; private init; }
    public string QuestionText { get; private init; } = string.Empty;
    public Guid RoundId { get; private init; }
    public Round Round { get; private init; } = null!;
    public ICollection<Answer> Answers { get; private set; } = null!;

    private Question(Guid id, int questionNumber, int fullScore, string questionText, Round round) : base(id)
    {
        QuestionNumber = questionNumber;
        FullScore = fullScore;
        QuestionText = questionText;
        Round = round;
        Answers = new List<Answer>();
    }

    public static Question Create(Guid id, int questionNumber, int fullScore, string questionText, Round round)
    {
        var Question = new Question(id, questionNumber, fullScore, questionText, round);

        //ToDo validation

        return Question;
    }

    public void AddAnswer(Answer answer)
    {
        //ToDo validation

        Answers.Add(answer);
    }
}
