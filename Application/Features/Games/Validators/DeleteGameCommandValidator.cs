using Application.Contracts.Persistence;
using Application.Features.Games.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Games.Validators
{
    public class DeleteGameCommandValidator : AbstractValidator<DeleteGameCommand>
    {
        private readonly IGameRepository _gameRepository;

        public DeleteGameCommandValidator(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;

            RuleFor(dgc => dgc.GameId)
                .Cascade(CascadeMode.Stop)
                .ValidGuid()
                .MustAsync(async (gi, token) => await _gameRepository.Exists(Guid.Parse(gi)))
                    .WithMessage("{PropertyValue} nem létezik")
                .OverridePropertyName("gameId");
        }
    }
}
