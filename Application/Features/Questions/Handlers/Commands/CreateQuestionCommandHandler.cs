using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Questions.Requests.Commands;
using Application.Features.Questions.Validators;
using Application.Features.Rounds.Validators;
using Application.MappingProfiles;
using CSharpFunctionalExtensions;
using Domain.Games;
using MediatR;

namespace Application.Features.Questions.Handlers.Commands;

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, QuestionResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQuestionRepository _questionRepository;
    private readonly IRoundRepository _roundRepository;

    public CreateQuestionCommandHandler(IUnitOfWork unitOfWork, IQuestionRepository questionRepository, IRoundRepository roundRepository)
    {
        _unitOfWork = unitOfWork;
        _questionRepository = questionRepository;
        _roundRepository = roundRepository;
    }

    public async Task<QuestionResponseDTO> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateQuestionCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        Maybe<Round?> round = await _roundRepository.Get(Guid.Parse(request.QuestionRequestDTO.RoundId));
        if (round.HasNoValue)
            throw new QuizValidationException("Some vaidation error occcurs", "roundId", "Round does not exist");

        var questionsOfRound = await _questionRepository.GetQuestionsOfRoundAsync(request.QuestionRequestDTO.RoundId);

        var question = request.QuestionRequestDTO.ToQuestion(questionsOfRound.Count + 1);

        await _questionRepository.Add(question);
        await _unitOfWork.Save();

        return question.ToQuestionResponseDTO();
    }
}
