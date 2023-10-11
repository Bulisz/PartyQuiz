using Application.Contracts.Persistence.Base;
using Application.Features.Rounds.Requests.Commands;
using MediatR;

namespace Application.Features.Rounds.Handlers.Commands;

public class DeleteRoundCommandHandler : IRequestHandler<DeleteRoundCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoundCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteRoundCommand request, CancellationToken cancellationToken)
    {
        var round = await _unitOfWork.RoundRepository.Get(Guid.Parse(request.RoundId));

        if (round is not null)
        {
            _unitOfWork.RoundRepository.Delete(round);
            await _unitOfWork.Save();
        }
    }
}
