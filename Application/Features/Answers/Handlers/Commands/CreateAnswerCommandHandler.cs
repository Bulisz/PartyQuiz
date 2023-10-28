using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Answers.Requests.Commands;
using Application.Features.Answers.Validators;
using Application.MappingProfiles;
using CSharpFunctionalExtensions;
using Domain.Games;
using MediatR;

namespace Application.Features.Answers.Handlers.Commands;

public class CreateAnswerCommandHandler : IRequestHandler<CreateAnswerCommand, AnswerResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQuestionRepository _questionRepository;
    private readonly IRoundRepository _roundRepository;

    public CreateAnswerCommandHandler(IUnitOfWork unitOfWork, IQuestionRepository questionRepository, IRoundRepository roundRepository)
    {
        _unitOfWork = unitOfWork;
        _questionRepository = questionRepository;
        _roundRepository = roundRepository;
    }

    public async Task<AnswerResponseDTO> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateAnswerCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        Maybe<Question?> question = await _questionRepository.Get(Guid.Parse(request.AnswerRequestDTO.QuestionId));
        if (question.HasNoValue)
            throw new QuizValidationException("Some vaidation error occcurs", "questionId", "Question id does not exist");

        Maybe<Round?> round = await _roundRepository.Get(Guid.Parse(request.AnswerRequestDTO.RoundId));
        if (round.HasNoValue)
            throw new QuizValidationException("Some vaidation error occcurs", "roundId", "Round id does not exist");

        var answer = request.AnswerRequestDTO.ToAnswer();

        var addQuestionResult = question.Value!.TryToAddAnswer(answer, round.Value!.RoundType);
        if (addQuestionResult.IsFailure)
            throw new QuizValidationException("Some vaidation error occcurs", "isCorrect", addQuestionResult.Error);

        _questionRepository.Update(question.Value);
        await _unitOfWork.Save();

        return answer.ToAnswerResponseDTO();
    }
}
