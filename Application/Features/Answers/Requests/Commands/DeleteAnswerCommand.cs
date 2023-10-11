using MediatR;

namespace Application.Features.Answers.Requests.Commands;

public class DeleteAnswerCommand : IRequest
{
    public string AnswerId { get; }
    public DeleteAnswerCommand(string answerId)
    {
        AnswerId = answerId;
    }
}
