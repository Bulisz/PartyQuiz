using Application.DTOs;
using MediatR;

namespace Application.Features.Games.Request.Commands;

public class UpdateGameCommand : IRequest
{
    public GameUpdateDTO GameUpdateDTO { get; private init; }

    public UpdateGameCommand(GameUpdateDTO gameUpdateDTO)
    {
        GameUpdateDTO = gameUpdateDTO;
    }
}
