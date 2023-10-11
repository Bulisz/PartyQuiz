using Application.Contracts.Persistence.Base;
using Application.Features.Answers.Requests.Commands;
using MediatR;

namespace Application.Features.Answers.Handlers.Commands;

public class DeleteAnswerCommandHandler : IRequestHandler<DeleteAnswerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAnswerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        var answer = await _unitOfWork.AnswerRepository.Get(Guid.Parse(request.AnswerId));

        if (answer is not null)
        {
            _unitOfWork.AnswerRepository.Delete(answer);
            await _unitOfWork.Save();
        }
    }
}
