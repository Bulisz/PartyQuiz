using Application.Contracts.Persistence;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Answers.Requests.Queries;
using Application.Features.Answers.Validators;
using Application.MappingProfiles;
using CSharpFunctionalExtensions;
using Domain.Games;
using MediatR;

namespace Application.Features.Answers.Handlers.Queries;

public class GetAnswersOfQuestionQueryHandler : IRequestHandler<GetAnswersOfQuestionQuery, List<AnswerResponseDTO>>
{
    private readonly IAnswerRepository _answerRepository;
    private readonly IQuestionRepository _questionRepository;

    public GetAnswersOfQuestionQueryHandler(IAnswerRepository answerRepository, IQuestionRepository questionRepository)
    {
        _answerRepository = answerRepository;
        _questionRepository = questionRepository;
    }

    public async Task<List<AnswerResponseDTO>> Handle(GetAnswersOfQuestionQuery request, CancellationToken cancellationToken)
    {
        var validator = new GetAnswersOfQuestionQueryValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new QuizValidationException("Some validation error occurs", validationResult.Errors);

        Maybe<Question?> question = await _questionRepository.Get(Guid.Parse(request.QuestionId));
        if (question.HasNoValue)
            throw new QuizValidationException("Some validation error occurs", "questionId", "Question does not exist");

        var answers = await _answerRepository.GetAnswersOfQuestionAsync(request.QuestionId);

        return answers.Select(a => a.ToAnswerResponseDTO()).ToList();
    }
}
