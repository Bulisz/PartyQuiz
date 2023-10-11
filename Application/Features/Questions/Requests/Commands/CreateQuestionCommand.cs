using Application.DTOs;
using MediatR;

namespace Application.Features.Questions.Requests.Commands;

public class CreateQuestionCommand : IRequest<QuestionResponseDTO>
{
    public QuestionRequestDTO QuestionRequestDTO { get; }
    public CreateQuestionCommand(QuestionRequestDTO questionRequestDTO)
    {
        QuestionRequestDTO = questionRequestDTO;
    }
}
