using Application.Contracts.Persistence.Base;
using Application.Features.Questions.Requests.Commands;
using MediatR;

namespace Application.Features.Questions.Handlers.Commands;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteQuestionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.QuestionRepository.Get(Guid.Parse(request.QuestionId));

        if (question is not null)
        {
            _unitOfWork.QuestionRepository.Delete(question);
            await _unitOfWork.Save();
        }
    }
}
