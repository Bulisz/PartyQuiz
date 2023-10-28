using Application.Contracts.Persistence;
using Application.Features.Games.Requests.Commands;
using Application.Helpers;
using FluentValidation;

namespace Application.Features.Games.Validators
{
    public class DeleteGameCommandValidator : AbstractValidator<DeleteGameCommand>
    {
        public DeleteGameCommandValidator()
        {
            RuleFor(dgc => dgc.GameId)
                .ValidGuid()
                .OverridePropertyName("gameId");
        }
    }
}
