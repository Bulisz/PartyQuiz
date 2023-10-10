using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Features.Rounds.Requests.Queries;
using Application.MappingProfiles;
using MediatR;

namespace Application.Features.Rounds.Handlers.Queries;

public class GetRoundsOfGameQueryHandler : IRequestHandler<GetRoundsOfGameQuery, List<RoundResponseDTO>>
{
    private readonly IRoundRepository _roundRepository;

    public GetRoundsOfGameQueryHandler(IRoundRepository roundRepository)
    {
        _roundRepository = roundRepository;
    }

    public async Task<List<RoundResponseDTO>> Handle(GetRoundsOfGameQuery request, CancellationToken cancellationToken)
    {
        var rounds = await _roundRepository.GetRoundsOfGameAsync(request.GameId);

        return rounds.Select(r => r.ToRoundResponseDTO()).ToList();
    }
}
