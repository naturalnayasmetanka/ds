using Serilog;
using Serilog.Exceptions;

namespace DS.Presentation.DI
{
    public static class LoggingDI
    {
        public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSerilog((serviceProvider, lo) => lo
                .ReadFrom.Configuration(configuration)
                .ReadFrom.Services(serviceProvider)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("ServiceName", "DS"));

            return services;
        }
    }
}
