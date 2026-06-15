using DS.Application.Departments.Exceptions;
using DS.Application.DepartmentsLocations.Exceptions;
using DS.Application.Locations.Exceptions;
using DS.Domain.Exceptions;
using System.Text.Json;

namespace DS.Presentation.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, exception.Message);

        (int code, Errors? errors) = exception switch
        {
            DepartmentNotFoundException => (StatusCodes.Status404NotFound, JsonSerializer.Deserialize<Errors>(exception.Message)),

            LocationNotFoundException => (StatusCodes.Status404NotFound, JsonSerializer.Deserialize<Errors>(exception.Message)),
            LocationDuplicatieException => (StatusCodes.Status409Conflict, JsonSerializer.Deserialize<Errors>(exception.Message)),

            DepartmentLocationNotFoundException => (StatusCodes.Status404NotFound, JsonSerializer.Deserialize<Errors>(exception.Message)),
            DepartmentLocationBindAlreadyExistsException => (StatusCodes.Status409Conflict, JsonSerializer.Deserialize<Errors>(exception.Message)),

            _ => (StatusCodes.Status500InternalServerError, new List<Error> { Error.Failure(null, "Something wrong") })
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;

        await context.Response.WriteAsJsonAsync(errors);
    }
}

public static class ExceptionMiddlewareExtention
{
    public static IApplicationBuilder UseExceptionMiddleware(this WebApplication app) =>
        app.UseMiddleware<ExceptionMiddleware>();
}