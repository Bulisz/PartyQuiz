using Application.DTOs;
using CSharpFunctionalExtensions;
using MediatR;

namespace Application.Features.Games.Requests.Commands;

public class CreateGameCommand : IRequest<GameResponseDTO>
{
    public GameRequestDTO GameRequestDTO { get; }

    public CreateGameCommand(GameRequestDTO gameRequestDTO)
    {
        GameRequestDTO = gameRequestDTO;
    }
}
