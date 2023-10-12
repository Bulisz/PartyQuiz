using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Games.Requests.Commands;
using Application.Features.Games.Validators;
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
        var validator = new CreateGameCommandValidator(_unitOfWork.GameRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs",validationResult.Errors);

        var game = request.GameRequestDTO.ToGame();

        await _unitOfWork.GameRepository.Add(game);
        await _unitOfWork.Save();

        return game.ToGameResponseDTO()!;
    }
}
