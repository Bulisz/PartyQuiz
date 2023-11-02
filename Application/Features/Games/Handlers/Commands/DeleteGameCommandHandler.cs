using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Games.Requests.Commands;
using Application.Features.Games.Validators;
using CSharpFunctionalExtensions;
using Domain.Games;
using MediatR;

namespace Application.Features.Games.Handlers.Commands;

public class DeleteGameCommandHandler : IRequestHandler<DeleteGameCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameRepository _gameRepository;

    public DeleteGameCommandHandler(IUnitOfWork unitOfWork, IGameRepository gameRepository)
    {
        _unitOfWork = unitOfWork;
        _gameRepository = gameRepository;
    }

    public async Task Handle(DeleteGameCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteGameCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new QuizValidationException("Some validation error occurs", validationResult.Errors);

        Maybe<Game?> game = await _gameRepository.Get(Guid.Parse(request.GameId));
        if (game.HasNoValue)
            throw new QuizValidationException("Some validation error occurs", "gameId", "Game Id does not exist");

        _gameRepository.Delete(game.Value!);
        await _unitOfWork.Save();
    }
}
