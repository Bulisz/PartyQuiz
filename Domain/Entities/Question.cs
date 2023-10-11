using Domain.Entities.Base;

namespace Domain.Entities;

public sealed class Question : BaseEntity
{
    public int QuestionNumber { get; set; }
    public int FullScore { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public Guid RoundId { get; private init; }
    public Round Round { get; private init; } = null!;
    public ICollection<Answer> Answers { get; private set; } = null!;

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

        //ToDo validation

        return Question;
    }

    public void AddAnswer(Answer answer)
    {
        //ToDo validation

        Answers.Add(answer);
    }
}
