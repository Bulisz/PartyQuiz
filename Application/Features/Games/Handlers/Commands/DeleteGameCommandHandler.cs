using Application.Contracts.Persistence.Base;
using Application.Features.Games.Requests.Commands;
using MediatR;

namespace Application.Features.Games.Handlers.Commands;

public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGameCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteGameCommand request, CancellationToken cancellationToken)
    {
        var game = await _unitOfWork.GameRepository.Get(Guid.Parse(request.GameId));

        if (game is not null)
        {
            _unitOfWork.GameRepository.Delete(game);
            await _unitOfWork.Save();
        }
    }
}
