using DS.Application.Departments.Repositories;
using DS.Application.Departments.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DS.Application.Departments;

public static class DI
{
    public static IServiceCollection AddDepartments(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

       
        services.AddValidatorsFromAssembly(typeof(DI).Assembly);

        services.AddScoped<IDepartmantsService, DepartmentsService>();


        return services;
    }
}
