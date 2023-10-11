using MediatR;

namespace Application.Features.Rounds.Requests.Commands;

public class DeleteRoundCommand : IRequest
{
    public string RoundId { get; }
    public DeleteRoundCommand(string roundId)
    {
        RoundId = roundId;
    }
}
