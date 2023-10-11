using MediatR;

namespace Application.Features.Questions.Requests.Commands;

public class DeleteQuestionCommand : IRequest
{
    public string QuestionId { get; }
    public DeleteQuestionCommand(string questionId)
    {
        QuestionId = questionId;
    }
}
