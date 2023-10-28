using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Answers.Requests.Commands;
using Application.Features.Answers.Validators;
using CSharpFunctionalExtensions;
using Domain.Games;
using MediatR;

namespace Application.Features.Answers.Handlers.Commands;

public class DeleteAnswerCommandHandler : IRequestHandler<DeleteAnswerCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnswerRepository _answerRepository;

    public DeleteAnswerCommandHandler(IUnitOfWork unitOfWork, IAnswerRepository answerRepository)
    {
        _unitOfWork = unitOfWork;
        _answerRepository = answerRepository;
    }

    public async Task Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        var validator = new DeleteAnswerCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        Maybe<Answer?> answer = await _answerRepository.Get(Guid.Parse(request.AnswerId));
        if (answer.HasNoValue)
            throw new QuizValidationException("Some vaidation error occcurs", "answerId", "Answer Id does not exist");

        _answerRepository.Delete(answer.Value!);
        await _unitOfWork.Save();
    }
}
