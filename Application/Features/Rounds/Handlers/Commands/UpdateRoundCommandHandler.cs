using Application.Contracts.Persistence.Base;
using Application.Features.Rounds.Requests.Commands;
using Domain.Enums;
using MediatR;

namespace Application.Features.Rounds.Handlers.Commands;

public class UpdateRoundCommandHandler : IRequestHandler<UpdateRoundCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoundCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateRoundCommand request, CancellationToken cancellationToken)
    {
        var round = await _unitOfWork.RoundRepository.Get(Guid.Parse(request.RoundUpdateDTO.Id));

        if (round is not null)
        {
            round.RoundNumber = request.RoundUpdateDTO.RoundNumber;
            round.RoundName = request.RoundUpdateDTO.RoundName;
            round.RoundType = Enum.Parse<RoundType>(request.RoundUpdateDTO.RoundType);

            _unitOfWork.RoundRepository.Update(round);
            await _unitOfWork.Save();
        }
    }
}
