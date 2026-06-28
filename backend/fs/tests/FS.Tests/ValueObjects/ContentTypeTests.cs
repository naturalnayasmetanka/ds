using FluentAssertions;
using FS.Core.Enums;
using FS.Core.ValueObjects;

namespace FS.Tests.ValueObjects;

public class ContentTypeTests
{
    [Theory]
    [InlineData("image/jpeg", MediaType.IMAGE)]
    [InlineData("image/png", MediaType.IMAGE)]
    [InlineData("video/mp4", MediaType.VIDEO)]
    [InlineData("audio/mpeg", MediaType.AUDIO)]
    [InlineData("application/pdf", MediaType.DOCUMENT)]
    public void Create_WithKnownMimeType_ShouldReturnCorrectCategory(string mime, MediaType expected)
    {
        var result = ContentType.Create(mime);

        result.IsSuccess.Should().BeTrue();
        result.Value.Category.Should().Be(expected);
    }

    [Fact]
    public void Create_WithUnknownMimeType_ShouldReturnUnknown()
    {
        var result = ContentType.Create("application/unknown");

        result.IsSuccess.Should().BeTrue();
        result.Value.Category.Should().Be(MediaType.UNKNOWN);
    }

    [Fact]
    public void Create_WithEmptyString_ShouldFail()
    {
        var result = ContentType.Create("");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("empty.contenttype");
    }

    [Fact]
    public void Create_ShouldNormalizeToLowerCase()
    {
        var result = ContentType.Create("Image/JPEG");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("image/jpeg");
    }
}