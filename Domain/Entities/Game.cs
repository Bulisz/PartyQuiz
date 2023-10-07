using Domain.Entities.Base;

namespace Domain.Entities;

public sealed class Game : BaseEntity
{
    public string GameName { get; private init; } = string.Empty;
    public ICollection<Round> Rounds { get; private set; } = null!;

    private Game(Guid id, string gameName) : base(id)
    {
        GameName = gameName;
        Rounds = new List<Round>();
    }

    public static Game Create(Guid id, string gameName)
    {
        var game = new Game(id, gameName);

        //ToDo validation

        return game;
    }

    public void AddRound(Round round) => Rounds.Add(round);
}
