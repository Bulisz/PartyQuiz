using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Features.Games.Request.Queries;
using Application.MappingProfiles;
using MediatR;

namespace Application.Features.Games.Handlers.Queries;

public class GetAllGameQueryHandler : IRequestHandler<GetAllGameQuery, List<GameResponseDTO>>
{
    private readonly IGameRepository _repository;

    public GetAllGameQueryHandler(IGameRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<GameResponseDTO>> Handle(GetAllGameQuery request, CancellationToken cancellationToken)
    {
        var games = await _repository.GetAll();

        return games.Select(g => g.ToGameResponseDTO()).ToList();
    }
}
