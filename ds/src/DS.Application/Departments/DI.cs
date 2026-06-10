using DS.Application.Abstractions.Handlers;
using DS.Application.Departments.Handlers.Commands.Create;
using DS.Application.Departments.Handlers.Commands.Update;
using DS.Application.Departments.Handlers.Queries.GetBy;
using DS.Contracts.Departments.GetById;
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

        services.AddScoped<IQueryHandler<GetDepartmentResponse?, GetDepartmentQuery>, GetDepartmentHandler>();

        return services;
    }
}
