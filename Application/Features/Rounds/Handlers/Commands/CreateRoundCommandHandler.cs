using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Features.Rounds.Requests.Commands;
using Application.MappingProfiles;
using MediatR;

namespace Application.Features.Rounds.Handlers.Commands;

public class CreateRoundCommandHandler : IRequestHandler<CreateRoundCommand, RoundResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoundCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RoundResponseDTO> Handle(CreateRoundCommand request, CancellationToken cancellationToken)
    {
        var round = request.RoundRequestDTO.ToRound();

        await _unitOfWork.RoundRepository.Add(round);
        await _unitOfWork.Save();

        return round.ToRoundResponseDTO();
    }
}
