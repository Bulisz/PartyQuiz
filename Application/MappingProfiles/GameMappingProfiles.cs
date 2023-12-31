﻿using Application.DTOs;
using Domain.Games;

namespace Application.MappingProfiles;

public static class GameMappingProfiles
{
    public static Game ToGame(this GameRequestDTO GameRequestDTO) =>
        Game.Create(GameRequestDTO.GameName).Value;

    public static GameResponseDTO? ToGameResponseDTO(this Game? game)
    {
        if (game is not null)
        {
            var roundList = game.Rounds.Select(r => r.ToRoundResponseDTO()).ToList();

            return new GameResponseDTO(
                game.Id.ToString(),
                game.GameName,
                roundList);
        }
        else
            return null;
    }
}
