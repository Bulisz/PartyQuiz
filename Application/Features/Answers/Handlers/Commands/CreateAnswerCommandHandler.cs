using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Exceptions;
using Application.Features.Answers.Requests.Commands;
using Application.Features.Answers.Validators;
using Application.MappingProfiles;
using MediatR;

namespace Application.Features.Answers.Handlers.Commands;

public class CreateAnswerCommandHandler : IRequestHandler<CreateAnswerCommand, AnswerResponseDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateAnswerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AnswerResponseDTO> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateAnswerCommandValidator(_unitOfWork.AnswerRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        var answer = request.AnswerRequestDTO.ToAnswer();

        await _unitOfWork.AnswerRepository.Add(answer);
        await _unitOfWork.Save();

        return answer.ToAnswerResponseDTO();
    }
}
