using Application.DTOs;
using MediatR;

namespace Application.Features.Answers.Requests.Commands;

public class CreateAnswerCommand : IRequest<AnswerResponseDTO>
{
    public AnswerRequestDTO AnswerRequestDTO { get; }

    public CreateAnswerCommand(AnswerRequestDTO answerRequestDTO)
    {
        AnswerRequestDTO = answerRequestDTO;
    }
}
