using Application.Contracts.Persistence.Base;
using Application.DTOs;
using Application.Features.Questions.Requests.Commands;
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
        var question = request.QuestionRequestDTO.ToQuestion();

        await _unitOfWork.QuestionRepository.Add(question);
        await _unitOfWork.Save();

        return question.ToQuestionResponseDTO();
    }
}
