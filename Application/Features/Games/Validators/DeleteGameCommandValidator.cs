using Application.Contracts.Persistence;
using Application.Features.Games.Requests.Commands;
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
                .Must(gi => Guid.TryParse(gi, out _))
                .WithMessage("{PropertyValue} is not valid Id")
                .MustAsync(async (gi, token) => await _gameRepository.Exists(Guid.Parse(gi)))
                .WithMessage("{PropertyValue} does not exists")
                .OverridePropertyName("gameId");
        }
    }
}
