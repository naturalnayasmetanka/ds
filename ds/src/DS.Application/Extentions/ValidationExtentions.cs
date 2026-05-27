using CSharpFunctionalExtensions;
using DS.Domain.Exceptions;
using FluentValidation.Results;

namespace DS.Application.Extentions
{
    public static class ValidationExtentions
    {
        public static List<Error> ToErrorList(this ValidationResult validationResult)
            => validationResult.Errors.Select(e => Error.Validation(e.ErrorCode, e.ErrorMessage, e.PropertyName)).ToList();

        public static List<Error> ToErrorList(this Result result, string code)
            => new List<Error>() { Error.Failure(code, result.Error) };

        public static Result<T, List<Error>> ToErrorList<T>(this Result result, string code)
            => Result.Failure<T, List<Error>>(new List<Error>() { Error.Failure(code, result.Error) });
    }
}
