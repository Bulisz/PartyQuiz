using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Games.Requests.Queries;
using Application.MappingProfiles;
using CSharpFunctionalExtensions;
using Domain.Games;
using MediatR;

namespace Application.Features.Games.Handlers.Queries;

public class GetGameByNameQueryHandler : IRequestHandler<GetGameByNameQuery, GameResponseDTO>
{
    private readonly IGameRepository _repository;

    public GetGameByNameQueryHandler(IGameRepository repository)
    {
        _repository = repository;
    }

    public async Task<GameResponseDTO> Handle(GetGameByNameQuery request, CancellationToken cancellationToken)
    {
        Maybe<Game?> game = await _repository.GetGameByNameAsync(request.GameName);
        if (game.HasNoValue)
            throw new QuizValidationException("Some vaidation error occcurs", "gameName", "Game name does not exist");

        return game.Value.ToGameResponseDTO()!;
    }
}
