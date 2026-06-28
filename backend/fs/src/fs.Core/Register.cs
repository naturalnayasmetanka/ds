using FS.Core.Abstractions.Common;
using FS.Core.Features;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using static CSharpFunctionalExtensions.Result;

namespace FS.Core;

public static class Register
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<Guid, StartMultipartUploadCommand>, StartMultipartUploadHandler>();

        return services;
    }
}