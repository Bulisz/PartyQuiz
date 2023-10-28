using Application.Contracts.Persistence;
using Application.Features.Answers.Requests.Commands;
using Application.Helpers;
using Domain.Enums;
using FluentValidation;

namespace Application.Features.Answers.Validators;

public class CreateAnswerCommandValidator : AbstractValidator<CreateAnswerCommand>
{
    public CreateAnswerCommandValidator()
    {
        RuleFor(cac => cac.AnswerRequestDTO.AnswerText)
            .NotEmpty()
            .OverridePropertyName("answerText");

        RuleFor(cac => cac.AnswerRequestDTO.RoundId)
            .ValidGuid()
            .OverridePropertyName("roundId");

        RuleFor(cac => cac.AnswerRequestDTO.QuestionId)
            .ValidGuid()
            .OverridePropertyName("questionId");
    }
}