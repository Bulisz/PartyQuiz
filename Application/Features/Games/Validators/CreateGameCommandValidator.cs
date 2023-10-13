using Application.Contracts.Persistence;
using Application.Features.Games.Requests.Commands;
using FluentValidation;

namespace Application.Features.Games.Validators;

public class CreateGameCommandValidator : AbstractValidator<CreateGameCommand>
{
    private readonly IGameRepository _gameRepository;

    public CreateGameCommandValidator(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;

        RuleFor(cgc => cgc.GameRequestDTO.GameName)
            .MustAsync(async (gn, token) => await _gameRepository.GetGameByNameAsync(gn) is null)
                .WithMessage("{PropertyValue} már létezik")
            .NotEmpty()
            .OverridePropertyName("gameName");
    }
}
