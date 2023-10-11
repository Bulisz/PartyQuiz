using Application.DTOs;
using MediatR;

namespace Application.Features.Answers.Requests.Queries;

public class GetAnswersOfQuestionQuery : IRequest<List<AnswerResponseDTO>>
{
    public string QuestionId { get; }
    public GetAnswersOfQuestionQuery(string questionId)
    {
        QuestionId = questionId;
    }
}
