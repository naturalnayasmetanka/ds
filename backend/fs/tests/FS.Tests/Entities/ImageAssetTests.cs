using FluentAssertions;
using FS.Core.Entities;
using FS.Core.Enums;
using FS.Core.ValueObjects;

namespace FS.Tests.Entities;

public class ImageAssetTests
{
    private MediaData CreateValidMediaData(
        string fileName = "photo.jpg",
        string contentType = "image/jpeg",
        long size = 1024,
        int chunks = 1)
    {
        var fn = FileName.Create(fileName).Value;
        var ct = ContentType.Create(contentType).Value;
        return MediaData.Create(fn, ct, size, chunks).Value;
    }

    [Fact]
    public void Validate_WithValidData_ShouldSucceed()
    {
        var mediaData = CreateValidMediaData();

        var result = ImageAsset.Validate(mediaData);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithInvalidExtension_ShouldFail()
    {
        var mediaData = CreateValidMediaData("video.mp4", "image/jpeg");

        var result = ImageAsset.Validate(mediaData);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("image.invalid.extension");
    }

    [Fact]
    public void Validate_WithVideoContentType_ShouldFail()
    {
        var mediaData = CreateValidMediaData("photo.jpg", "video/mp4");

        var result = ImageAsset.Validate(mediaData);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("image.invalid.content-type");
    }

    [Fact]
    public void Validate_WithSizeExceedingMax_ShouldFail()
    {
        var mediaData = CreateValidMediaData(size: ImageAsset.MAX_SIZE + 1);

        var result = ImageAsset.Validate(mediaData);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("image.invalid.size");
    }

    [Fact]
    public void CreateForUpload_WithValidData_ShouldReturnUploadingStatus()
    {
        var mediaData = CreateValidMediaData();

        var result = ImageAsset.CreateForUpload(Guid.NewGuid(), mediaData);

        result.IsSuccess.Should().BeTrue();
        result.Value.MediaStatus.Should().Be(MediaStatus.UPLOADING);
        result.Value.Key.Should().NotBeNull();
    }
}