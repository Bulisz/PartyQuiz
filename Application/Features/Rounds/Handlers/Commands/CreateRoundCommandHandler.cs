using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Rounds.Requests.Commands;
using Application.Features.Rounds.Validators;
using Application.MappingProfiles;
using CSharpFunctionalExtensions;
using Domain.Games;
using MediatR;

namespace Application.Features.Rounds.Handlers.Commands;

public class CreateRoundCommandHandler : IRequestHandler<CreateRoundCommand, RoundResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameRepository _gameRepository;

    public CreateRoundCommandHandler(IUnitOfWork unitOfWork, IGameRepository gameRepository)
    {
        _unitOfWork = unitOfWork;
        _gameRepository = gameRepository;
    }

    public async Task<RoundResponseDTO> Handle(CreateRoundCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateRoundCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        Maybe<Game?> game = await _gameRepository.Get(Guid.Parse(request.RoundRequestDTO.GameId));
        if (game.HasNoValue)
            throw new QuizValidationException("Some vaidation error occcurs", "gameId", "Game id does not exist");

        var round = request.RoundRequestDTO.ToRound();

        var addRoundResult = game.Value!.TryToAddRound(round);
        if (addRoundResult.IsFailure)
            throw new QuizValidationException("Some vaidation error occcurs", "round", addRoundResult.Error);

        _gameRepository.Update(game.Value);
        await _unitOfWork.Save();

        return round.ToRoundResponseDTO();
    }
}
