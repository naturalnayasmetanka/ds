using DS.Domain.Enums;
using DS.Domain.Exceptions;

namespace DS.Presentation.Results
{
    public sealed class ErrorResult : IResult
    {
        private readonly Errors _errors;

        public ErrorResult(Error error)
        {
            _errors = new List<Error>() { error };
        }

        public ErrorResult(Errors errors)
        {
            _errors = errors;
        }

        public Task ExecuteAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            if (!_errors.Any())
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                return httpContext.Response.WriteAsJsonAsync(Envelope.Error(_errors));
            }

            var distErrTypes = _errors.Select(e => e.ErrorType).Distinct().ToList();

            int statusCode = distErrTypes.Count > 1
                ? StatusCodes.Status500InternalServerError
                : GetStatusCodeFOrErrorType(distErrTypes.First());

            var envelope = Envelope.Error(_errors);
            httpContext.Response.StatusCode = statusCode;

            return httpContext.Response.WriteAsJsonAsync(envelope);
        }

        private static int GetStatusCodeFOrErrorType(ErrorType errorType)
            => errorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
    }
}
