using DS.Infrastructure;

namespace DS.Presentation.DI;

public static class InfrastructureDI
{
    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services)
    {
        services.AddInfrastractureDb();

        return services;
    }
}
