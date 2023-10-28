using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Rounds.Requests.Commands;
using Application.Features.Rounds.Validators;
using Application.MappingProfiles;
using CSharpFunctionalExtensions;
using Domain.Games;
using MediatR;

namespace Application.Features.Rounds.Handlers.Commands;

public class UpdateRoundCommandHandler : IRequestHandler<UpdateRoundCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameRepository _gameRepository;

    public UpdateRoundCommandHandler(IUnitOfWork unitOfWork, IGameRepository gameRepository)
    {
        _unitOfWork = unitOfWork;
        _gameRepository = gameRepository;
    }

    public async Task Handle(UpdateRoundCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateRoundCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        Maybe<Game?> game = await _gameRepository.Get(Guid.Parse(request.RoundUpdateDTO.GameId));
        if (game.HasNoValue)
            throw new QuizValidationException("Some vaidation error occcurs", "gameId", "Game id does not exist");

        Maybe<Round?> roundExist = game.Value!.Rounds.FirstOrDefault(r => r.Id == Guid.Parse(request.RoundUpdateDTO.Id));
        if (roundExist.HasNoValue)
            throw new QuizValidationException("Some vaidation error occcurs", "roundId", "Round id does not exist");

        var roundToModify = request.RoundUpdateDTO.ToRound();
        roundToModify.Modify(Guid.Parse(request.RoundUpdateDTO.Id));

        var roundModifyResult = game.Value.TryToModifyRoundOfGame(roundToModify);
        if (roundModifyResult.IsFailure)
            throw new QuizValidationException("Some vaidation error occcurs", roundModifyResult.GetType().ToString(), roundModifyResult.Error);

        _gameRepository.Update(game.Value);
        await _unitOfWork.Save();
    }
}
