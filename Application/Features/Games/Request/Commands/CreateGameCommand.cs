using Application.DTOs;
using MediatR;

namespace Application.Features.Games.Request.Commands;

public class CreateGameCommand : IRequest<GameResponseDTO>
{
    public GameRequestDTO GameRequestDTO { get; private init; }

    public CreateGameCommand(GameRequestDTO gameRequestDTO)
    {
        GameRequestDTO = gameRequestDTO;
    }
}
