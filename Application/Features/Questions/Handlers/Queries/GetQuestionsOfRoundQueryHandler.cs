using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Features.Questions.Requests.Queries;
using Application.MappingProfiles;
using MediatR;

namespace Application.Features.Questions.Handlers.Queries;

public class GetQuestionsOfRoundQueryHandler : IRequestHandler<GetQuestionsOfRoundQuery, List<QuestionResponseDTO>>
{
    private readonly IQuestionRepository _questionRepository;

    public GetQuestionsOfRoundQueryHandler(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<List<QuestionResponseDTO>> Handle(GetQuestionsOfRoundQuery request, CancellationToken cancellationToken)
    {
        var questions = await _questionRepository.GetQuestionsOfRoundAsync(request.RoundId);

        return questions.Select(q => q.ToQuestionResponseDTO()).ToList();
    }
}
