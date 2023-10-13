using Domain.Enums;
using FluentValidation;

namespace Application.Helpers;

public static class MyCustomValidators
{
    public static IRuleBuilderOptions<T, string> ValidGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(guid => Guid.TryParse(guid, out _)).WithMessage("{PropertyValue} nem valós Id");
    }

    public static IRuleBuilderOptions<T, string> ValidRoundType<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(roundType => Enum.TryParse<RoundType>(roundType, out _)).WithMessage("{PropertyValue} nem valós kör típus");
    }
}