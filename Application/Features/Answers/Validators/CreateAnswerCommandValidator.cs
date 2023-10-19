using Application.Contracts.Persistence;
using Application.Features.Answers.Requests.Commands;
using Domain.Enums;
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

        RuleFor(cac => cac.AnswerRequestDTO.IsCorrect)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (ctx, at, token) =>
                {
                    var roundType = ctx.AnswerRequestDTO.RoundType;
                    var answersOfQuestion = await _answerRepository.GetAnswersOfQuestionAsync(ctx.AnswerRequestDTO.QuestionId);
                    return Enum.Parse<RoundType>(roundType) == RoundType.ABCD || answersOfQuestion.Count == 0;
                })
                .WithMessage("Erre a kör típusra csak egy válasz lehetséges")
            .Must(ic => ic).WhenAsync(async (ctx, ic, token) =>
                {
                    var answersOfQuestion = await _answerRepository.GetAnswersOfQuestionAsync(ctx.AnswerRequestDTO.QuestionId);
                    return answersOfQuestion.Count == 0;
                })
                .WithMessage("Az első válasz legyen helyes")
            .Must(ic => !ic).WhenAsync(async (ctx, ic, token) =>
                {
                    var answersOfQuestion = await _answerRepository.GetAnswersOfQuestionAsync(ctx.AnswerRequestDTO.QuestionId);
                    return answersOfQuestion.Count > 0;
                })
                .WithMessage("Csak az első válasz lehet helyes")
            .MustAsync(async (ctx, at, token) =>
                {
                    var answersOfQuestion = await _answerRepository.GetAnswersOfQuestionAsync(ctx.AnswerRequestDTO.QuestionId);
                    return answersOfQuestion.Count == 5;
                })
                .WithMessage("Összesen max 5 válasz adható")
            .OverridePropertyName("isCorrect");
    }
}