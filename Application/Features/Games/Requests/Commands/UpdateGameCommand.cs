using Application.DTOs;
using MediatR;

namespace Application.Features.Games.Requests.Commands;

public class UpdateGameCommand : IRequest
{
    public GameUpdateDTO GameUpdateDTO { get; }

    public UpdateGameCommand(GameUpdateDTO gameUpdateDTO)
    {
        GameUpdateDTO = gameUpdateDTO;
    }
}
