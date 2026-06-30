using Amazon.S3;
using FS.Core.Abstractions;
using FS.Core.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FS.Infrastructure.S3;

public static class Register
{
    public static IServiceCollection AddS3Infrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<S3Options>(configuration.GetSection("S3"));

        services.AddScoped<IS3Provider, S3Provider>();

        services.AddSingleton<IAmazonS3>(sp =>
        {
            S3Options s3Options = sp.GetRequiredService<IOptions<S3Options>>().Value;

            var config = new AmazonS3Config
            {
                ServiceURL = s3Options.Endpoint,
                UseHttp = !s3Options.WithSsl,
                ForcePathStyle = true,
            };
            return new AmazonS3Client(s3Options.AccessKey, s3Options.SecretKey, config);
        });

        services.AddHostedService<S3BuckerInitializationService>();

        services.AddTransient<IChunkSizeCalculator, ChunkSizeCalculator>();

        return services;
    }
}

