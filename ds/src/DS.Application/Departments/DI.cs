using DS.Application.Abstractions;
using DS.Application.Departments.Handlers.Create;
using DS.Application.Departments.Handlers.Update;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DS.Application.Departments;

public static class DI
{
    public static IServiceCollection AddDepartments(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);


        services.AddValidatorsFromAssembly(typeof(DI).Assembly);

        services.AddScoped<ICommandHandler<Guid, CreateDepartmentCommand>, CreateDepartmentHandler>();
        services.AddScoped<ICommandHandler<Guid, UpdateDepartmentCommand>, UpdateDepartmentHandler>();

        return services;
    }
}
