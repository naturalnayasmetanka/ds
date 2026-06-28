using CSharpFunctionalExtensions;
using FluentAssertions;
using FS.Core.Entities;
using FS.Core.Enums;
using FS.Core.Exceptions;
using FS.Core.ValueObjects;

namespace FS.Tests.ValueObjects;

public class FileNameTests
{
    [Fact]
    public void Create_WithValidFileName_ShouldSucceed()
    {
        var result = FileName.Create("photo.jpg");

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("photo");
        result.Value.Extension.Should().Be("jpg");
    }

    [Fact]
    public void Create_WithEmptyFileName_ShouldFail()
    {
        var result = FileName.Create("");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("empty.filename");
    }

    [Fact]
    public void Create_WithoutExtension_ShouldFail()
    {
        var result = FileName.Create("photo");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("empty.extension");
    }

    [Fact]
    public void Create_WithDotAtStart_ShouldFail()
    {
        var result = FileName.Create(".gitignore");

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Create_WithDotAtEnd_ShouldFail()
    {
        var result = FileName.Create("photo.");

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Create_ExtensionShouldBeLowerCase()
    {
        var result = FileName.Create("photo.JPG");

        result.IsSuccess.Should().BeTrue();
        result.Value.Extension.Should().Be("jpg");
    }
}