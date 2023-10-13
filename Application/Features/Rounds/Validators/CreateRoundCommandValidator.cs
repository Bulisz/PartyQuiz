using Application.Contracts.Persistence;
using Application.Features.Rounds.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Rounds.Validators;

public class CreateRoundCommandValidator : AbstractValidator<CreateRoundCommand>
{
    private readonly IRoundRepository _roundRepository;

    public CreateRoundCommandValidator(IRoundRepository roundRepository)
    {
        _roundRepository = roundRepository;

        RuleFor(crc => crc.RoundRequestDTO.RoundNumber)
            .GreaterThan(0).WithMessage("A kör száma legyen nagyobb mint 0")
            .LessThanOrEqualTo(10).WithMessage("A maximum kör szám 10")
            .MustAsync(async (ctx, rn, token) =>
                (await _roundRepository.GetRoundsOfGameAsync(ctx.RoundRequestDTO.GameId)).All(r => r.RoundNumber != rn))
                .WithMessage("Ilyen számú kör már van")
            .OverridePropertyName("roundNumber");

        RuleFor(crc => crc.RoundRequestDTO.RoundName)
            .NotEmpty()
            .MustAsync(async (ctx, rn, token) =>
                (await _roundRepository.GetRoundsOfGameAsync(ctx.RoundRequestDTO.GameId)).All(r => r.RoundName != rn))
                .WithMessage("Ilyen nevű kör már van")
            .OverridePropertyName("roundName");

        RuleFor(crc => crc.RoundRequestDTO.RoundType)
            .ValidRoundType()
            .OverridePropertyName("roundType");
    }
}