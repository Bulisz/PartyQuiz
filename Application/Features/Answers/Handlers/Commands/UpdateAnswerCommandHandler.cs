﻿using Application.Contracts.Persistence.Base;
using Application.Exceptions;
using Application.Features.Answers.Requests.Commands;
using Application.Features.Answers.Validators;
using MediatR;

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
        var validator = new UpdateAnswerCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new QuizValidationException("Some vaidation error occcurs", validationResult.Errors);

        var answer = await _unitOfWork.AnswerRepository.Get(Guid.Parse(request.AnswerUpdateDTO.Id));

        if (answer is not null)
        {
            answer.Modify(request.AnswerUpdateDTO.AnswerText);

            _unitOfWork.AnswerRepository.Update(answer);
            await _unitOfWork.Save();
        }
    }
}
