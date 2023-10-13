using Application.Contracts.Persistence;
using Application.Features.Answers.Requests.Commands;
using FluentValidation;

namespace Application.Features.Answers.Validators;

public class CreateAnswerCommandValidator : AbstractValidator<CreateAnswerCommand>
{
    private readonly IAnswerRepository _answerRepository;

    public CreateAnswerCommandValidator(IAnswerRepository answerRepository)
    {
        _answerRepository = answerRepository;

        RuleFor(cac => cac.AnswerRequestDTO.AnswerText)
            .NotEmpty()
            .OverridePropertyName("answerText");
    }
}