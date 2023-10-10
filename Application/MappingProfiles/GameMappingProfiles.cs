using Application.DTOs;
using Domain.Entities;

namespace Application.MappingProfiles;

public static class GameMappingProfiles
{
    public static Game ToGame(this GameRequestDTO GameRequestDTO)
    {
        return Game.Create(GameRequestDTO.GameName);
    }

    public static GameResponseDTO ToGameResponseDTO(this Game Game)
    {
        //ToDo roundlist

        return new GameResponseDTO(Game.Id.ToString(), Game.GameName, Enumerable.Empty<RoundResponseDTO>().ToList());
    }
}
