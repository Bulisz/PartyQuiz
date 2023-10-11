using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Features.Answers.Requests.Queries;
using Application.MappingProfiles;
using MediatR;

namespace Application.Features.Answers.Handlers.Queries;

public class GetAnswersOfQuestionQueryHandler : IRequestHandler<GetAnswersOfQuestionQuery, List<AnswerResponseDTO>>
{
    private readonly IAnswerRepository _answerRepository;

    public GetAnswersOfQuestionQueryHandler(IAnswerRepository answerRepository)
    {
        _answerRepository = answerRepository;
    }

    public async Task<List<AnswerResponseDTO>> Handle(GetAnswersOfQuestionQuery request, CancellationToken cancellationToken)
    {
        var answers = await _answerRepository.GetAnswersOfQuestionAsync(request.QuestionId);

        return answers.Select(a => a.ToAnswerResponseDTO()).ToList();
    }
}
