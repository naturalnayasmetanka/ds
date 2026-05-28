using CSharpFunctionalExtensions;
using DS.Domain.Exceptions;
using FluentValidation;
using System.Text.Json;

namespace DS.Domain.Validation;

public static class CustomValidatiors
{
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject, Errors>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            Result<TValueObject, Errors> result = factoryMethod.Invoke(value);

            if (result.IsSuccess)
                return;

            context.AddFailure(JsonSerializer.Serialize(result.Error));
        });
    }

    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, Errors error)
    {
        return rule.WithMessage(JsonSerializer.Serialize(error));
    }
}
