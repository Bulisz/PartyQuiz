using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Features.Answers.Requests.Commands;
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
        var answer = request.AnswerRequestDTO.ToAnswer();

        await _unitOfWork.AnswerRepository.Add(answer);
        await _unitOfWork.Save();

        return answer.ToAnswerResponseDTO();
    }
}
