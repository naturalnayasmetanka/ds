using DS.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace DS.Presentation.DI;

public static class InfrastructureDI
{
    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastractureDb(configuration);

        return services;
    }
}
