using CSharpFunctionalExtensions;
using FS.Core.Abstractions;
using FS.Core.Exceptions;
using FS.Core.Options;
using Microsoft.Extensions.Options;

namespace FS.Infrastructure.S3;

public class ChunkSizeCalculator : IChunkSizeCalculator
{
    private readonly IOptions<S3Options> _options;

    public ChunkSizeCalculator(IOptions<S3Options> options)
    {
        _options = options;
    }

    public Result<(long ChunkSize, int TotalChunks), Error> Calculate(
        long fileSize)
    {
        if (_options.Value.ChunkSize <= 0 || _options.Value.MaxChunks <= 0)
            return Result.Failure<(long ChunkSize, int TotalChunks), Error>(
                Error.Validation("calculate.chunks.fail", "calculate.chunks.fail"));

        if (fileSize <= _options.Value.ChunkSize)
            return Result.Failure<(long ChunkSize, int TotalChunks), Error>(
                Error.Validation("calculate.chunks.fail", "calculate.chunks.fail"));

        int calculatedChunks = (int)Math.Ceiling((double)fileSize / _options.Value.ChunkSize);

        int actualChunks = Math.Min(calculatedChunks, _options.Value.MaxChunks);

        long chunkSize = (fileSize + actualChunks - 1) / actualChunks;

        return Result.Success<(long ChunkSize, int TotalChunks), Error>((chunkSize, actualChunks));
    }
}
