using Application.DTOs;
using MediatR;

namespace Application.Features.Games.Requests.Commands;

public class UpdateGameCommand : IRequest
{
    public GameUpdateDTO GameUpdateDTO { get; private init; }

    public UpdateGameCommand(GameUpdateDTO gameUpdateDTO)
    {
        GameUpdateDTO = gameUpdateDTO;
    }
}
