using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Games.Requests.Commands;
using Application.Features.Games.Validators;
using CSharpFunctionalExtensions;
using Domain.Games;
using MediatR;

namespace Application.Features.Games.Handlers.Commands;

public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameRepository _gameRepository;

    public UpdateGameCommandHandler(IUnitOfWork unitOfWork, IGameRepository gameRepository)
    {
        _unitOfWork = unitOfWork;
        _gameRepository = gameRepository;
    }

    public async Task Handle(UpdateGameCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateGameCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new QuizValidationException("Some validation error occurs", validationResult.Errors);

        Maybe<Game?> gameNameExist = await _gameRepository.GetGameByNameAsync(request.GameUpdateDTO.GameName);
        if (gameNameExist.HasValue)
            throw new QuizValidationException("Some validation error occurs", "gameName", "Game name already exist");

        Maybe<Game?> game = await _gameRepository.Get(Guid.Parse(request.GameUpdateDTO.Id));
        if (game.HasNoValue)
            throw new QuizValidationException("Some validation error occurs", "gameId", "This game id does not exist");

        game.Value!.Modify(request.GameUpdateDTO.GameName);

        _gameRepository.Update(game.Value!);
        await _unitOfWork.Save();
    }
}
