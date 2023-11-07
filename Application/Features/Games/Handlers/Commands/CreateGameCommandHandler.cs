using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Games.Requests.Commands;
using Application.Features.Games.Validators;
using Application.MappingProfiles;
using CSharpFunctionalExtensions;
using Domain.Games;
using MediatR;

namespace Application.Features.Games.Handlers.Commands;

public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, GameResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameRepository _gameRepository;

    public CreateGameCommandHandler(IUnitOfWork unitOfWork, IGameRepository gameRepository)
    {
        _unitOfWork = unitOfWork;
        _gameRepository = gameRepository;
    }

    public async Task<GameResponseDTO> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateGameCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new QuizValidationException("Some validation error occurs", validationResult.Errors);

        Maybe<Game?> gameNameExists = await _gameRepository.GetGameByNameAsync(request.GameRequestDTO.GameName);
        if (gameNameExists.HasValue)
            throw new QuizValidationException("Some validation error occurs", "gameName", "Game name already exist");

        var game = request.GameRequestDTO.ToGame();

        await _gameRepository.Add(game);
        await _unitOfWork.Save();

        return game.ToGameResponseDTO()!;
    }
}
