﻿using Domain.Entities.Base;

namespace Domain.Entities;

public sealed class Game : BaseEntity
{
    public string GameName { get; set; } = string.Empty;
    public ICollection<Round> Rounds { get; private init; } = null!;

    private Game(string gameName)
    {
        GameName = gameName;
        Rounds = new List<Round>();
    }

    public static Game Create(string gameName)
    {
        var game = new Game(gameName);

        return game;
    }
}
