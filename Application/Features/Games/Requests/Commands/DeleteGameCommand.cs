using MediatR;

namespace Application.Features.Games.Requests.Commands;

public class DeleteGameCommand : IRequest
{
    public string GameId  { get; }
    public DeleteGameCommand(string gameId)
    {
        GameId = gameId;
    }
}
