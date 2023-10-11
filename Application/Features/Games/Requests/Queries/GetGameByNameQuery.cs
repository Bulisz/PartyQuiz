using Application.DTOs;
using MediatR;

namespace Application.Features.Games.Requests.Queries;

public class GetGameByNameQuery : IRequest<GameResponseDTO>
{
    public string GameName { get; }
    public GetGameByNameQuery(string gameName)
    {
        GameName = gameName;
    }
}