using FluentAssertions;
using FS.Core.Entities;
using FS.Core.Enums;
using FS.Core.ValueObjects;

namespace FS.Tests.Entities;

public class MediaAssetStateMachineTests
{
    private ImageAsset CreateUploadingAsset()
    {
        var fileName = FileName.Create("photo.jpg").Value;
        var contentType = ContentType.Create("image/jpeg").Value;
        var mediaData = MediaData.Create(fileName, contentType, 1024, 1).Value;
        var owner = MediaOwner.Create("lesson", Guid.NewGuid()).Value;

        return new ImageAsset(Guid.NewGuid(), mediaData, MediaStatus.UPLOADING, owner);
    }

    [Fact]
    public void MarkUploaded_FromUploading_ShouldSucceed()
    {
        var asset = CreateUploadingAsset();

        var result = asset.MarkUploaded();

        result.IsSuccess.Should().BeTrue();
        asset.MediaStatus.Should().Be(MediaStatus.UPLOADED);
    }

    [Fact]
    public void MarkUploaded_FromReady_ShouldFail()
    {
        var asset = CreateUploadingAsset();
        asset.MarkUploaded();
        asset.MarkReady(new StorageKey("images", "raw", "file123"));

        var result = asset.MarkUploaded();

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("invalid.transition");
    }

    [Fact]
    public void MarkReady_FromUploaded_ShouldSucceed()
    {
        var asset = CreateUploadingAsset();
        asset.MarkUploaded();
        var key = new StorageKey("images", "raw", "file123");

        var result = asset.MarkReady(key);

        result.IsSuccess.Should().BeTrue();
        asset.MediaStatus.Should().Be(MediaStatus.READY);
        asset.Key.Should().Be(key);
    }

    [Fact]
    public void MarkReady_FromUploading_ShouldFail()
    {
        var asset = CreateUploadingAsset();
        var key = new StorageKey("images", "raw", "file123");

        var result = asset.MarkReady(key);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("invalid.transition");
    }

    [Fact]
    public void MarkFailed_FromUploading_ShouldSucceed()
    {
        var asset = CreateUploadingAsset();

        var result = asset.MarkFailed();

        result.IsSuccess.Should().BeTrue();
        asset.MediaStatus.Should().Be(MediaStatus.FAILED);
    }

    [Fact]
    public void MarkFailed_FromUploaded_ShouldSucceed()
    {
        var asset = CreateUploadingAsset();
        asset.MarkUploaded();

        var result = asset.MarkFailed();

        result.IsSuccess.Should().BeTrue();
        asset.MediaStatus.Should().Be(MediaStatus.FAILED);
    }

    [Fact]
    public void MarkFailed_FromReady_ShouldFail()
    {
        var asset = CreateUploadingAsset();
        asset.MarkUploaded();
        asset.MarkReady(new StorageKey("images", "raw", "file123"));

        var result = asset.MarkFailed();

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("invalid.transition");
    }

    [Fact]
    public void Delete_FromAnyStatus_ShouldSucceed()
    {
        var asset = CreateUploadingAsset();

        var result = asset.Delete();

        result.IsSuccess.Should().BeTrue();
        asset.MediaStatus.Should().Be(MediaStatus.DELETED);
    }

    [Fact]
    public void Delete_WhenAlreadyDeleted_ShouldFail()
    {
        var asset = CreateUploadingAsset();
        asset.Delete();

        var result = asset.Delete();

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("invalid.transition");
    }
}