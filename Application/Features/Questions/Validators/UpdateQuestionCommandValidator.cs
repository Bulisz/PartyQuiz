using Application.Features.Questions.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Questions.Validators;

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(uqc => uqc.QuestionUpdateDTO.Id)
            .ValidGuid()
            .OverridePropertyName("questionId");

        RuleFor(uqc => uqc.QuestionUpdateDTO.FullScore)
            .GreaterThan(0)
            .LessThanOrEqualTo(5)
            .OverridePropertyName("fullScore");

        RuleFor(uqc => uqc.QuestionUpdateDTO.QuestionText)
            .NotEmpty()
            .OverridePropertyName("questionText");
    }
}