using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Features.Games.Request.Queries;
using Application.MappingProfiles;
using MediatR;
using Presentation.Controllers;

namespace Application.Features.Games.Handlers.Queries;

public class GetGameByNameQueryHandler : IRequestHandler<GetGameByNameQuery, GameResponseDTO?>
{
    private readonly IGameRepository _repository;

    public GetGameByNameQueryHandler(IGameRepository repository)
    {
        _repository = repository;
    }

    public async Task<GameResponseDTO?> Handle(GetGameByNameQuery request, CancellationToken cancellationToken)
    {
        var game = await _repository.GetGameByNameAsync(request.GameName);

        return game.ToGameResponseDTO();
    }
}
