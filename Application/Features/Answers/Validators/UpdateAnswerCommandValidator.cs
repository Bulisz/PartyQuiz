using Application.Contracts.Persistence;
using Application.Features.Answers.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Answers.Validators;

public class UpdateAnswerCommandValidator: AbstractValidator<UpdateAnswerCommand>
{
    public UpdateAnswerCommandValidator()
    {
        RuleFor(uac => uac.AnswerUpdateDTO.Id)
            .ValidGuid()
            .OverridePropertyName("answerId");

        RuleFor(uac => uac.AnswerUpdateDTO.AnswerText)
            .NotEmpty()
            .OverridePropertyName("answerText");
    }
}