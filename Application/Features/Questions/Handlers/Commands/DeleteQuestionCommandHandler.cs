using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Questions.Requests.Commands;
using Application.Features.Questions.Validators;
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
        var validator = new DeleteQuestionCommandValidator(_unitOfWork.QuestionRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        var question = await _unitOfWork.QuestionRepository.Get(Guid.Parse(request.QuestionId));

        _unitOfWork.QuestionRepository.Delete(question!);
        await _unitOfWork.Save();
    }
}
