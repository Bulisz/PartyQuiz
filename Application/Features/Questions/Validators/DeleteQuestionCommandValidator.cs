using Application.Features.Questions.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Questions.Validators;

public class DeleteQuestionCommandValidator: AbstractValidator<DeleteQuestionCommand>
{

    public DeleteQuestionCommandValidator()
    {
        RuleFor(dqc => dqc.QuestionId)
            .ValidGuid()
            .OverridePropertyName("questionId");
    }
}