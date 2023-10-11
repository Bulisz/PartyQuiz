using Application.DTOs;
using MediatR;

namespace Application.Features.Answers.Requests.Commands;

public class UpdateAnswerCommand : IRequest
{
    public AnswerUpdateDTO AnswerUpdateDTO { get; }
    public UpdateAnswerCommand(AnswerUpdateDTO answerUpdateDTO)
    {
        AnswerUpdateDTO = answerUpdateDTO;
    }
}
