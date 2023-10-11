using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Features.Games.Requests.Queries;
using Application.MappingProfiles;
using MediatR;

namespace Application.Features.Games.Handlers.Queries;

public class GetAllGamesQueryHandler : IRequestHandler<GetAllGamesQuery, List<GameResponseDTO>>
{
    private readonly IGameRepository _repository;

    public GetAllGamesQueryHandler(IGameRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<GameResponseDTO>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
    {
        var games = await _repository.GetAll();

        return games.Select(g => g.ToGameResponseDTO()).ToList()!;
    }
}
