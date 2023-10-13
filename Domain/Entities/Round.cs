using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public sealed class Round : BaseEntity
{
    public int RoundNumber { get; private set; }
    public string RoundName { get; private set; } = string.Empty;
    public RoundType RoundType { get; private set; }
    public Guid GameId { get; private init; }
    public Game Game { get; private init; } = null!;
    public ICollection<Question> Questions { get; private init; } = null!;

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

        return round;
    }

    public void Modify(int roundNumber, string roundName, string roundType)
    {
        RoundNumber = roundNumber;
        RoundName = roundName;
        RoundType = Enum.Parse<RoundType>(roundType);
    }
}
