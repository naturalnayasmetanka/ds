using CSharpFunctionalExtensions;
using FS.Core.Exceptions;

namespace FS.Core.Abstractions;

public interface IChunkSizeCalculator
{
    Result<(long ChunkSize, int TotalChunks), Error> Calculate(long fileSize);
}