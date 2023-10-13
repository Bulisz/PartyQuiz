using Application.Contracts.Persistence;
using Application.Features.Answers.Requests.Commands;
using FluentValidation;

namespace Application.Features.Answers.Validators;

public class UpdateAnswerCommandValidator: AbstractValidator<UpdateAnswerCommand>
{
    private readonly IAnswerRepository _answerRepository;

    public UpdateAnswerCommandValidator(IAnswerRepository answerRepository)
    {
        _answerRepository = answerRepository;

        RuleFor(cac => cac.AnswerUpdateDTO.AnswerText)
            .NotEmpty()
            .OverridePropertyName("answerText");
    }
}