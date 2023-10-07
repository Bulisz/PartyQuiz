using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public sealed class Round : BaseEntity
{
    public int RoundNumber { get; private init; }
    public string RoundName { get; private init; } = string.Empty;
    public RoundType RoundType { get; private init; }
    public Guid GameId { get; private init; }
    public Game Game { get; private init; } = null!;
    public ICollection<Question> Questions { get; private set; } = null!;

    private Round(Guid id, int roundNumber, string roundName, RoundType roundType, Game game) : base(id)
    {
        RoundNumber = roundNumber;
        RoundName = roundName;
        RoundType = roundType;
        Game = game;
        Questions = new List<Question>();
    }

    public static Round Create(Guid id, int roundNumber, string roundName, RoundType roundType, Game game)
    {
        var round = new Round(id, roundNumber, roundName, roundType, game);

        //ToDo validation

        return round;
    }

    public void AddQuestion(Question question)
    {
        //ToDo validation
        Questions.Add(question);
    }
}
