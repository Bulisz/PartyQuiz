using Application.Features.Questions.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Questions.Validators;

public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(cqc => cqc.QuestionRequestDTO.RoundId)
            .ValidGuid()
            .OverridePropertyName("roundId");

        RuleFor(cqc => cqc.QuestionRequestDTO.FullScore)
            .GreaterThan(0)
            .LessThanOrEqualTo(5)
            .OverridePropertyName("fullScore");

        RuleFor(cqc => cqc.QuestionRequestDTO.QuestionText)
            .NotEmpty()
            .OverridePropertyName("questionText");
    }
}