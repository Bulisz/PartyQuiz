using Application.Contracts.Persistence;
using Application.Features.Rounds.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Rounds.Validators;

public class UpdateRoundCommandValidator : AbstractValidator<UpdateRoundCommand>
{
    private readonly IRoundRepository _roundRepository;

    public UpdateRoundCommandValidator(IRoundRepository roundRepository)
    {
        _roundRepository = roundRepository;

        RuleFor(urc => urc.RoundUpdateDTO.RoundNumber)
            .GreaterThan(0).WithMessage("A kör száma legyen nagyobb mint 0")
            .LessThanOrEqualTo(10).WithMessage("A maximum kör szám 10")
            .MustAsync(async (ctx, rn, token) =>
                {
                    var originalRound = await _roundRepository.Get(Guid.Parse(ctx.RoundUpdateDTO.Id));
                    var roundsOfGame = await _roundRepository.GetRoundsOfGameAsync(ctx.RoundUpdateDTO.GameId);
                    if (originalRound!.RoundNumber == ctx.RoundUpdateDTO.RoundNumber)
                    {
                        return true;
                    }
                    else if (roundsOfGame.All(r => r.RoundNumber != ctx.RoundUpdateDTO.RoundNumber))
                    {
                        return true;
                    }
                    else
                        return false;
                })
                .WithMessage("Ilyen számú kör már van")
            .OverridePropertyName("roundNumber");

        RuleFor(crc => crc.RoundUpdateDTO.RoundName)
            .NotEmpty()
            .MustAsync(async (ctx, rn, token) =>
                {
                    var originalRound = await _roundRepository.Get(Guid.Parse(ctx.RoundUpdateDTO.Id));
                    var roundsOfGame = await _roundRepository.GetRoundsOfGameAsync(ctx.RoundUpdateDTO.GameId);
                    if (originalRound!.RoundName == ctx.RoundUpdateDTO.RoundName)
                    {
                        return true;
                    }
                    else if (roundsOfGame.All(r => r.RoundName != ctx.RoundUpdateDTO.RoundName))
                    {
                        return true;
                    }
                    else
                        return false;
                })
                .WithMessage("Ilyen nevű kör már van")
            .OverridePropertyName("roundName");

        RuleFor(crc => crc.RoundUpdateDTO.RoundType)
            .ValidRoundType()
            .OverridePropertyName("roundType");
    }
}