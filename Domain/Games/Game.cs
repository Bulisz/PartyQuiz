using CSharpFunctionalExtensions;
using Domain.Astractions;

namespace Domain.Games;

public class Game : AggregateRoot
{
    public string GameName { get; private set; } = string.Empty;
    public IReadOnlyCollection<Round> Rounds => _rounds.AsReadOnly();
    private readonly List<Round> _rounds = new();
    private Game(string gameName)
    {
        GameName = gameName;
    }

    public static Result<Game> Create(string gameName)
    {
        return new Game(gameName);
    }

    public Result Modify(string gameName)
    {
        GameName = gameName;

        return Result.Success();
    }

    public Result TryToAddRound(Round round)
    {
        if (_rounds.Any(r => r.RoundNumber == round.RoundNumber))
            return Result.Failure("Round number already exists in this game");

        if (_rounds.Any(r => r.RoundName == round.RoundName))
            return Result.Failure("Round name already exists in this game");

        _rounds.Add(round);

        return Result.Success();
    }

    public Result TryToModifyRoundOfGame(Round roundToModify)
    {
        var originalRound = Rounds.First(r => r.Id == roundToModify.Id);

        if (originalRound.RoundNumber != roundToModify.RoundNumber &&
            Rounds.Any(r => r.RoundNumber == roundToModify.RoundNumber))
                return Result.Failure("Round number already exist in this game");

        if (originalRound!.RoundName != roundToModify.RoundName &&
            Rounds.Any(r => r.RoundName == roundToModify.RoundName))
                return Result.Failure("Round name already exist in this game");

        _rounds.Remove(originalRound);
        _rounds.Add(roundToModify);

        return Result.Success();
    }
}