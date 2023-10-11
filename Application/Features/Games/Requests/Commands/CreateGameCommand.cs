using Application.DTOs;
using MediatR;

namespace Application.Features.Games.Requests.Commands;

public class CreateGameCommand : IRequest<GameResponseDTO>
{
    public GameRequestDTO GameRequestDTO { get; private init; }

    public CreateGameCommand(GameRequestDTO gameRequestDTO)
    {
        GameRequestDTO = gameRequestDTO;
    }
}
