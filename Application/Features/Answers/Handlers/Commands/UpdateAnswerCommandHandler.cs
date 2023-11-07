using Application.Contracts.Persistence;
using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Answers.Requests.Commands;
using Application.Features.Answers.Validators;
using CSharpFunctionalExtensions;
using Domain.Games;
using MediatR;
using System.Text;

namespace Application.Features.Answers.Handlers.Commands;

public class UpdateAnswerCommandHandler : IRequestHandler<UpdateAnswerCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnswerRepository _answerRepository;

    public UpdateAnswerCommandHandler(IUnitOfWork unitOfWork, IAnswerRepository answerRepository)
    {
        _unitOfWork = unitOfWork;
        _answerRepository = answerRepository;
    }

    public async Task Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateAnswerCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new QuizValidationException("Some validation error occurs", validationResult.Errors);

        Maybe<Answer?> answer = await _answerRepository.Get(Guid.Parse(request.AnswerUpdateDTO.Id));
        if (answer.HasNoValue)
            throw new QuizValidationException("Some validation error occurs", "answerId", "Answer id does not exist");

        _ = answer.Value!.Modify(request.AnswerUpdateDTO.AnswerText);

        _answerRepository.Update(answer.Value!);
        await _unitOfWork.Save();
    }
}
