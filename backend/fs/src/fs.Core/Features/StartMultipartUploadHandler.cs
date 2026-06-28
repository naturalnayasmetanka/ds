using CSharpFunctionalExtensions;
using FS.Contracts;
using FS.Core.Abstractions;
using FS.Core.Abstractions.Common;
using FS.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace FS.Core.Features;

public record StartMultipartUploadCommand(StartMultipartUploadRequest request) : ICommand;

public sealed class StartMultipartUploadHandler : ICommandHandler<Guid, StartMultipartUploadCommand>
{
    private readonly ILogger<StartMultipartUploadHandler> _logger;
    private readonly IS3Provider _s3Provider;

    public StartMultipartUploadHandler(ILogger<StartMultipartUploadHandler> logger, IS3Provider s3Provider)
    {
        _logger = logger;
        _s3Provider = s3Provider;
    }

    public async Task<Result<Guid, Errors>> Handle(StartMultipartUploadCommand command, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        return Result.Success<Guid, Errors>(Guid.CreateVersion7());
    }
}
