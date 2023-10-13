using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Questions.Requests.Commands;
using Application.Features.Questions.Validators;
using MediatR;

namespace Application.Features.Questions.Handlers.Commands;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateQuestionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateQuestionCommandValidator(_unitOfWork.QuestionRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        var question = await _unitOfWork.QuestionRepository.Get(Guid.Parse(request.QuestionUpdateDTO.Id));

        if (question is not null)
        {
            question.Modify(request.QuestionUpdateDTO.FullScore, request.QuestionUpdateDTO.QuestionText);

            _unitOfWork.QuestionRepository.Update(question);
            await _unitOfWork.Save();
        }
    }
}
