using CSharpFunctionalExtensions;
using FS.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace fs.Presentation.Results;

/// <summary>
/// Единая точка преобразования доменного Result/Errors в HTTP-ответ.
/// Используется во всех контроллерах вместо ручных BadRequest/NotFound,
/// чтобы бизнес-ошибки всегда возвращались через один envelope и без exceptions.
/// </summary>
public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T, Error> result)
        => result.IsSuccess
            ? new ObjectResult(Envelope<T>.Ok(result.Value)) { StatusCode = StatusCodes.Status200OK }
            : ToErrorActionResult(result.Error);

    public static IActionResult ToActionResult<T>(this Result<T, Errors> result)
        => result.IsSuccess
            ? new ObjectResult(Envelope<T>.Ok(result.Value)) { StatusCode = StatusCodes.Status200OK }
            : ToErrorActionResult(result.Error);

    private static IActionResult ToErrorActionResult(Errors errors)
    {
        if (!errors.Any())
            return new ObjectResult(Envelope.Error(errors)) { StatusCode = StatusCodes.Status500InternalServerError };

        var distinctErrorTypes = errors.Select(e => e.ErrorType).Distinct().ToList();

        int statusCode = distinctErrorTypes.Count > 1
            ? StatusCodes.Status500InternalServerError
            : GetStatusCodeForErrorType(distinctErrorTypes[0]);

        return new ObjectResult(Envelope.Error(errors)) { StatusCode = statusCode };
    }

    private static int GetStatusCodeForErrorType(ErrorType errorType)
        => errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
}
