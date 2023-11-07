using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Questions.Requests.Queries;
using Application.Features.Questions.Validators;
using Application.MappingProfiles;
using CSharpFunctionalExtensions;
using Domain.Games;
using MediatR;

namespace Application.Features.Questions.Handlers.Queries;

public class GetQuestionsOfRoundQueryHandler : IRequestHandler<GetQuestionsOfRoundQuery, List<QuestionResponseDTO>>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IRoundRepository _roundRepository;

    public GetQuestionsOfRoundQueryHandler(IQuestionRepository questionRepository, IRoundRepository roundRepository)
    {
        _questionRepository = questionRepository;
        _roundRepository = roundRepository;
    }

    public async Task<List<QuestionResponseDTO>> Handle(GetQuestionsOfRoundQuery request, CancellationToken cancellationToken)
    {
        var validator = new GetQuestionsOfRoundQueryValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new QuizValidationException("Some validation error occurs", validationResult.Errors);

        Maybe<Round?> round = await _roundRepository.Get(Guid.Parse(request.RoundId));
        if (round.HasNoValue)
            throw new QuizValidationException("Some validation error occurs", "roundId", "Round does not exist");

        var questions = await _questionRepository.GetQuestionsOfRoundAsync(request.RoundId);

        return questions.Select(q => q.ToQuestionResponseDTO()).ToList();
    }
}
