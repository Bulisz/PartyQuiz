using Application.Contracts.Persistence;
using Application.Features.Answers.Requests.Commands;
using FluentValidation;

namespace Application.Features.Answers.Validators;

public class UpdateAnswerCommandValidator: AbstractValidator<UpdateAnswerCommand>
{
    public UpdateAnswerCommandValidator()
    {
        RuleFor(cac => cac.AnswerUpdateDTO.AnswerText)
            .NotEmpty()
            .OverridePropertyName("answerText");
    }
}