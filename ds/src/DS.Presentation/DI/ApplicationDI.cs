using DS.Application.Departments;
using DS.Application.DepartmentsLocations;
using DS.Application.DepartmentsPositions;
using DS.Application.Locations;
using DS.Application.Positions;

namespace DS.Presentation.DI
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddLocations();
            services.AddDepartments();
            services.AddDepartmentsLocations();
            services.AddDepartmentsPositions();
            services.AddPositions();

            return services;
        }
    }
}
