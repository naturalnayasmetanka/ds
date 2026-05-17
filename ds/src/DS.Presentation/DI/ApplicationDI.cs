using DS.Application.Locations;

namespace DS.Presentation.DI
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddLocations();

            return services;
        }
    }
}
