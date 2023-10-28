using Application.Contracts.Persistence;
using Application.Features.Answers.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Answers.Validators;

public class DeleteAnswerCommandValidator : AbstractValidator<DeleteAnswerCommand>
{
    public DeleteAnswerCommandValidator()
    {
        RuleFor(dac => dac.AnswerId)
            .ValidGuid()
            .OverridePropertyName("answerId");
    }
}