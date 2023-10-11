using Application.Contracts.Persistence.Base;
using Application.Features.Answers.Requests.Commands;
using MediatR;
using System.IO.Pipes;

namespace Application.Features.Answers.Handlers.Commands;

public class UpdateAnswerCommandHandler : IRequestHandler<UpdateAnswerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAnswerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
    {
        var answer = await _unitOfWork.AnswerRepository.Get(Guid.Parse(request.AnswerUpdateDTO.Id));

        if (answer is not null)
        {
            answer.AnswerText = request.AnswerUpdateDTO.AnswerText;
            answer.IsCorrect = request.AnswerUpdateDTO.IsCorrect;

            _unitOfWork.AnswerRepository.Update(answer);
            await _unitOfWork.Save();
        }
    }
}
