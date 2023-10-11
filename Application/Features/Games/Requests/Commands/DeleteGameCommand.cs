using Application.Contracts.Persistence;
using MediatR;

namespace Application.Features.Games.Requests.Commands;

public class DeleteGameCommand : IRequest
{
    public string GameId  { get; private set; }
    public DeleteGameCommand(string gameId)
    {
        GameId = gameId;
    }
}
