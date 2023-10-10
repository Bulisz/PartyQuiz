using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public sealed class Round : BaseEntity
{
    public int RoundNumber { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public RoundType RoundType { get; set; }
    public Guid GameId { get; private init; }
    public Game Game { get; private init; } = null!;
    public ICollection<Question> Questions { get; private set; } = null!;

    private Round(int roundNumber, string roundName, RoundType roundType, Guid gameId)
    {
        RoundNumber = roundNumber;
        RoundName = roundName;
        RoundType = roundType;
        GameId = gameId;
        Questions = new List<Question>();
    }

    public static Round Create(int roundNumber, string roundName, RoundType roundType, Guid gameId)
    {
        var round = new Round(roundNumber, roundName, roundType, gameId);

        //ToDo validation

        return round;
    }

    public void AddQuestion(Question question)
    {
        //ToDo validation
        Questions.Add(question);
    }
}
