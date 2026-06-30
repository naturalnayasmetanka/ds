using FS.Contracts;
using FS.Core.Abstractions.Common;
using FS.Core.Features;
using Microsoft.Extensions.DependencyInjection;

namespace FS.Core;

public static class Register
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<StartMultipartUploadResponse, StartMultipartUploadCommand>, StartMultipartUploadHandler>();
        services.AddScoped<ICommandHandler<CompleteMultipartUploadResponse, CompleteMultipartUploadCommand>, CompleteMultipartUploadHandler>();

        services.AddScoped<ICommandHandler<InitUploadResponse, InitUploadCommand>, InitUploadHandler>();
        services.AddScoped<ICommandHandler<CompleteUploadResponse, CompleteUploadCommand>, CompleteUploadHandler>();
        services.AddScoped<IQueryHandler<GetMediaAssetUrlResponse, GetMediaAssetUrlQuery>, GetMediaAssetUrlHandler>();

        return services;
    }
}