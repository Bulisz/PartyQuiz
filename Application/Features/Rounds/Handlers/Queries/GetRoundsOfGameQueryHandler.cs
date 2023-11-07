using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Rounds.Requests.Queries;
using Application.Features.Rounds.Validators;
using Application.MappingProfiles;
using CSharpFunctionalExtensions;
using Domain.Games;
using MediatR;

namespace Application.Features.Rounds.Handlers.Queries;

public class GetRoundsOfGameQueryHandler : IRequestHandler<GetRoundsOfGameQuery, List<RoundResponseDTO>>
{
    private readonly IRoundRepository _roundRepository;
    private readonly IGameRepository _gameRepository;

    public GetRoundsOfGameQueryHandler(IRoundRepository roundRepository, IGameRepository gameRepository)
    {
        _roundRepository = roundRepository;
        _gameRepository = gameRepository;
    }

    public async Task<List<RoundResponseDTO>> Handle(GetRoundsOfGameQuery request, CancellationToken cancellationToken)
    {
        var validator = new GetRoundsOfGameQueryValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new QuizValidationException("Some validation error occurs", validationResult.Errors);

        Maybe<Game?> game = await _gameRepository.Get(Guid.Parse(request.GameId));
        if (game.HasNoValue)
            throw new QuizValidationException("Some validation error occurs", "gameId", "Game does not exist");

        var rounds = await _roundRepository.GetRoundsOfGameAsync(request.GameId);

        return rounds.Select(r => r.ToRoundResponseDTO()).ToList();
    }
}
