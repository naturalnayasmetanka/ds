using Microsoft.OpenApi;

namespace fs.Presentation.DI;

public static class WebDI
{
    public static IServiceCollection AddWebDI(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddControllers();

        return services;
    }
}
