using FluentAssertions;
using FS.Core.Entities;
using FS.Core.Enums;
using FS.Core.ValueObjects;

namespace FS.Tests.Entities;

public class MediaAssetStateMachineTests
{
    private ImageAsset CreateAssetWithStatus(MediaStatus status)
    {
        var fileName = FileName.Create("photo.jpg").Value;
        var contentType = ContentType.Create("image/jpeg").Value;
        var mediaData = MediaData.Create(fileName, contentType, 1024, 1).Value;
        var key = new StorageKey("images", "raw", "file123");

        var asset = new ImageAsset(Guid.NewGuid(), mediaData, MediaStatus.UPLOADING, key);

        switch (status)
        {
            case MediaStatus.UPLOADED:
                asset.MarkUploaded();
                break;
            case MediaStatus.READY:
                asset.MarkUploaded();
                asset.MarkReady(new StorageKey("images", "raw", "file123"));
                break;
            case MediaStatus.FAILED:
                asset.MarkFailed();
                break;
            case MediaStatus.DELETED:
                asset.Delete();
                break;
        }

        return asset;
    }

    [Fact]
    public void MarkUploaded_FromUploading_ShouldSucceed()
    {
        var asset = CreateAssetWithStatus(MediaStatus.UPLOADING);

        var result = asset.MarkUploaded();

        result.IsSuccess.Should().BeTrue();
        asset.MediaStatus.Should().Be(MediaStatus.UPLOADED);
    }

    [Fact]
    public void MarkUploaded_FromReady_ShouldFail()
    {
        var asset = CreateAssetWithStatus(MediaStatus.READY);

        var result = asset.MarkUploaded();

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("invalid.transition");
    }

    [Fact]
    public void MarkReady_FromUploaded_ShouldSucceed()
    {
        var asset = CreateAssetWithStatus(MediaStatus.UPLOADED);
        var key = new StorageKey("images", "raw", "file123");

        var result = asset.MarkReady(key);

        result.IsSuccess.Should().BeTrue();
        asset.MediaStatus.Should().Be(MediaStatus.READY);
        asset.Key.Should().Be(key);
    }

    [Fact]
    public void MarkReady_FromUploading_ShouldFail()
    {
        var asset = CreateAssetWithStatus(MediaStatus.UPLOADING);
        var key = new StorageKey("images", "raw", "file123");

        var result = asset.MarkReady(key);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("invalid.transition");
    }

    [Fact]
    public void MarkFailed_FromUploading_ShouldSucceed()
    {
        var asset = CreateAssetWithStatus(MediaStatus.UPLOADING);

        var result = asset.MarkFailed();

        result.IsSuccess.Should().BeTrue();
        asset.MediaStatus.Should().Be(MediaStatus.FAILED);
    }

    [Fact]
    public void MarkFailed_FromUploaded_ShouldSucceed()
    {
        var asset = CreateAssetWithStatus(MediaStatus.UPLOADED);

        var result = asset.MarkFailed();

        result.IsSuccess.Should().BeTrue();
        asset.MediaStatus.Should().Be(MediaStatus.FAILED);
    }

    [Fact]
    public void MarkFailed_FromReady_ShouldFail()
    {
        var asset = CreateAssetWithStatus(MediaStatus.READY);

        var result = asset.MarkFailed();

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("invalid.transition");
    }

    [Theory]
    [InlineData(MediaStatus.UPLOADING)]
    [InlineData(MediaStatus.UPLOADED)]
    [InlineData(MediaStatus.READY)]
    [InlineData(MediaStatus.FAILED)]
    public void Delete_FromAnyNonDeletedStatus_ShouldSucceed(MediaStatus status)
    {
        var asset = CreateAssetWithStatus(status);

        var result = asset.Delete();

        result.IsSuccess.Should().BeTrue();
        asset.MediaStatus.Should().Be(MediaStatus.DELETED);
    }

    [Fact]
    public void Delete_WhenAlreadyDeleted_ShouldFail()
    {
        var asset = CreateAssetWithStatus(MediaStatus.DELETED);

        var result = asset.Delete();

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("invalid.transition");
    }

    [Fact]
    public void CompleteUpload_WithMatchingActualData_ShouldTransitionToReady()
    {
        var asset = CreateAssetWithStatus(MediaStatus.UPLOADING);
        var actual = ActualMediaData.Create(1024, "image/jpeg", "etag-1").Value;

        var result = asset.CompleteUpload(actual);

        result.IsSuccess.Should().BeTrue();
        asset.MediaStatus.Should().Be(MediaStatus.READY);
        asset.ActualData.Should().Be(actual);
    }

    [Fact]
    public void CompleteUpload_WithMismatchedSize_ShouldFailAndMarkFailed()
    {
        var asset = CreateAssetWithStatus(MediaStatus.UPLOADING);
        var actual = ActualMediaData.Create(2048, "image/jpeg", "etag-1").Value;

        var result = asset.CompleteUpload(actual);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("image.mismatch.size");
        asset.MediaStatus.Should().Be(MediaStatus.FAILED);
    }

    [Fact]
    public void CompleteUpload_WithMismatchedContentType_ShouldFailAndMarkFailed()
    {
        var asset = CreateAssetWithStatus(MediaStatus.UPLOADING);
        var actual = ActualMediaData.Create(1024, "image/png", "etag-1").Value;

        var result = asset.CompleteUpload(actual);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("image.mismatch.content-type");
        asset.MediaStatus.Should().Be(MediaStatus.FAILED);
    }

    [Fact]
    public void CompleteUpload_WhenAlreadyReady_ShouldFailWithoutChangingState()
    {
        var asset = CreateAssetWithStatus(MediaStatus.READY);
        var actual = ActualMediaData.Create(1024, "image/jpeg", "etag-1").Value;

        var result = asset.CompleteUpload(actual);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("invalid.transition");
        asset.MediaStatus.Should().Be(MediaStatus.READY);
    }
}