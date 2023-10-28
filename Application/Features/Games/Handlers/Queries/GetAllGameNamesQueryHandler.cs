using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Features.Games.Requests.Queries;
using Application.MappingProfiles;
using CSharpFunctionalExtensions;
using MediatR;

namespace Application.Features.Games.Handlers.Queries;

public class GetAllGameNamesQueryHandler : IRequestHandler<GetAllGameNamesQuery, List<GameResponseDTO>>
{
    private readonly IGameRepository _repository;

    public GetAllGameNamesQueryHandler(IGameRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<GameResponseDTO>> Handle(GetAllGameNamesQuery request, CancellationToken cancellationToken)
    {
        var games = await _repository.GetAllGameNames();

        return games.Select(g => g.ToGameResponseDTO()).ToList()!;
    }
}
