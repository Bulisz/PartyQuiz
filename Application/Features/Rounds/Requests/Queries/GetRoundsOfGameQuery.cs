using Application.DTOs;
using MediatR;

namespace Application.Features.Rounds.Requests.Queries;

public class GetRoundsOfGameQuery : IRequest<List<RoundResponseDTO>>
{
    public string GameId { get; private init; }
    public GetRoundsOfGameQuery(string gameId)
    {
        GameId = gameId;
    }
}
