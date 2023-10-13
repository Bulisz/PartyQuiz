using Application.Contracts.Persistence;
using Application.Features.Games.Requests.Commands;
using FluentValidation;

namespace Application.Features.Games.Validators;

public class UpdateGameCommandValidator : AbstractValidator<UpdateGameCommand>
{
    private readonly IGameRepository _gameRepository;

    public UpdateGameCommandValidator(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;

        RuleFor(ugc => ugc.GameUpdateDTO.GameName)
            .MustAsync(async (gn, token) => await _gameRepository.GetGameByNameAsync(gn) is null)
                .WithMessage("{PropertyValue} már létezik")
            .NotEmpty()
            .OverridePropertyName("gameName");
    }
}
