using Application.DTOs;
using MediatR;

namespace Application.Features.Rounds.Requests.Commands;

public class CreateRoundCommand : IRequest<RoundResponseDTO>
{
    public RoundRequestDTO RoundRequestDTO { get; }

    public CreateRoundCommand(RoundRequestDTO roundRequestDTO)
    {
        RoundRequestDTO = roundRequestDTO;
    }
}
