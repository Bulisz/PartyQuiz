using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Features.Games.Requests.Commands;
using Application.MappingProfiles;
using MediatR;

namespace Application.Features.Games.Handlers.Commands;

public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, GameResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateGameCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GameResponseDTO> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var game = request.GameRequestDTO.ToGame();

        await _unitOfWork.GameRepository.Add(game);
        await _unitOfWork.Save();

        return game.ToGameResponseDTO()!;
    }
}
