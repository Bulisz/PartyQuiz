using Application.DTOs;
using MediatR;

namespace Application.Features.Questions.Requests.Commands;

public class UpdateQuestionCommand : IRequest
{
    public QuestionUpdateDTO QuestionUpdateDTO { get; }
    public UpdateQuestionCommand(QuestionUpdateDTO questionUpdateDTO)
    {
        QuestionUpdateDTO = questionUpdateDTO;
    }
}
