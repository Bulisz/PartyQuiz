using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Answers.Requests.Commands;
using Application.Features.Answers.Validators;
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
        var validator = new DeleteAnswerCommandValidator(_unitOfWork.AnswerRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        var answer = await _unitOfWork.AnswerRepository.Get(Guid.Parse(request.AnswerId));

        _unitOfWork.AnswerRepository.Delete(answer!);
        await _unitOfWork.Save();
    }
}
