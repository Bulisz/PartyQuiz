using CSharpFunctionalExtensions;
using Domain.Enums;

namespace Domain.Games;

public class Round : Entity<Guid>
{
    public int RoundNumber { get; private set; }
    public string RoundName { get; private set; } = string.Empty;
    public RoundType RoundType { get; private set; }
    public Guid GameId { get; private init; }
    public Game Game { get; private init; } = null!;
    public IReadOnlyCollection<Question> Questions => _questions;
    private readonly List<Question> _questions = new();

    private Round(int roundNumber, string roundName, RoundType roundType, Guid gameId)
    {
        RoundNumber = roundNumber;
        RoundName = roundName;
        RoundType = roundType;
        GameId = gameId;
    }

    public static Result<Round> Create(int roundNumber, string roundName, string roundType, Guid gameId)
    {
        if (!Enum.TryParse(roundType, out RoundType selectedRoundType))
            return Result.Failure<Round>("Invalid round type");

        var round = new Round(roundNumber, roundName, selectedRoundType, gameId);

        return round;
    }

    public Result Modify(Guid id)
    {
        Id = id;

        return Result.Success();
    }
}