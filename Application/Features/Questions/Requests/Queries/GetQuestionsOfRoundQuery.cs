using Application.DTOs;
using MediatR;

namespace Application.Features.Questions.Requests.Queries;

public class GetQuestionsOfRoundQuery : IRequest<List<QuestionResponseDTO>>
{
    public string RoundId { get; }
    public GetQuestionsOfRoundQuery(string roundId)
    {
        RoundId = roundId;
    }
}
