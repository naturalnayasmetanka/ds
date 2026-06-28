using FluentAssertions;
using FS.Core.ValueObjects;

namespace FS.Tests.ValueObjects;

public class StorageKeyTests
{
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        var result = StorageKey.Create("images", "raw", "file123");

        result.IsSuccess.Should().BeTrue();
        result.Value.FullPath.Should().Be("images/raw/file123");
        result.Value.Value.Should().Be("raw/file123");
    }

    [Fact]
    public void Create_WithEmptyPrefix_ShouldBuildPathWithoutPrefix()
    {
        var result = StorageKey.Create("images", null, "file123");

        result.IsSuccess.Should().BeTrue();
        result.Value.FullPath.Should().Be("images/file123");
        result.Value.Value.Should().Be("file123");
    }

    [Fact]
    public void Create_WithEmptyLocation_ShouldFail()
    {
        var result = StorageKey.Create("", null, "file123");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("empty.location");
    }

    [Fact]
    public void Create_WithEmptyKey_ShouldFail()
    {
        var result = StorageKey.Create("images", null, "");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("empty.value");
    }

    [Fact]
    public void Create_WithSlashInKey_ShouldFail()
    {
        var result = StorageKey.Create("images", null, "folder/file");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("invalid.key");
    }
}