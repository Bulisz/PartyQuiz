using Application.DTOs;
using MediatR;

namespace Presentation.Controllers;

public class GetGameByNameQuery : IRequest<GameResponseDTO>
{
    public string GameName { get; private init; }
    public GetGameByNameQuery(string gameName)
    {
        GameName = gameName;
    }
}