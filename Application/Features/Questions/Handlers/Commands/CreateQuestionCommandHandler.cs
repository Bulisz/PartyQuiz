using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Questions.Requests.Commands;
using Application.Features.Questions.Validators;
using Application.Features.Rounds.Validators;
using Application.MappingProfiles;
using MediatR;

namespace Application.Features.Questions.Handlers.Commands;

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, QuestionResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateQuestionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<QuestionResponseDTO> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateQuestionCommandValidator(_unitOfWork.QuestionRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        var questionsOfRound = await _unitOfWork.QuestionRepository.GetQuestionsOfRoundAsync(request.QuestionRequestDTO.RoundId);

        var question = request.QuestionRequestDTO.ToQuestion(questionsOfRound.Count + 1);

        await _unitOfWork.QuestionRepository.Add(question);
        await _unitOfWork.Save();

        return question.ToQuestionResponseDTO();
    }
}
