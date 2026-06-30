using CSharpFunctionalExtensions;
using FS.Core.Enums;
using FS.Core.Exceptions;
using FS.Core.ValueObjects;

namespace FS.Core.Entities;

public class ImageAsset : MediaAsset
{
    protected ImageAsset() { }

    public ImageAsset(Guid id, MediaData mediaData, MediaStatus mediaStatus, StorageKey key)
        : base(id, mediaData, mediaStatus, AssetType.IMAGE, key)
    {

    }

    public const long MAX_SIZE = 5_368_709_120;

    public const string LOCATION = "images";
    public const string RAW_PREFIX = "raw";
    public const string ALLOWED_CONTENT_TYPE = "image";

    public static readonly string[] AllowedExtensions = ["jpg", "jpeg", "png", "webp", "gif", "svg"];

    public static UnitResult<Error> Validate(MediaData mediaData)
    {
        if (!AllowedExtensions.Contains(mediaData.FileName.Extension))
        {
            return Error.Validation("image.invalid.extension", $"File extension must be one of: {string.Join(", ", AllowedExtensions)}");
        }

        if (mediaData.ContentType.Category != MediaType.IMAGE)
        {
            return Error.Validation("image.invalid.content-type", $"File content type must be {MediaType.IMAGE}");
        }

        if (mediaData.Size > MAX_SIZE)
        {
            return Error.Validation("image.invalid.size", $"File size must be less than {MAX_SIZE} bytes");
        }

        return UnitResult.Success<Error>();
    }

    public static Result<ImageAsset, Error> CreateForUpload(Guid id, MediaData mediaData)
    {
        var validationResult = Validate(mediaData);
        if (validationResult.IsFailure)
            return validationResult.Error;

        var key = StorageKey.GenerateForAsset(LOCATION, RAW_PREFIX);

        return Result.Success<ImageAsset, Error>(new ImageAsset(id, mediaData, MediaStatus.UPLOADING, key));
    }

    protected override UnitResult<Error> ValidateActual(ActualMediaData actual)
    {
        if (actual.Size != MediaData.Size)
            return Error.Conflict("image.mismatch.size",
                $"Uploaded file size {actual.Size} does not match declared size {MediaData.Size}");

        var actualContentTypeResult = ContentType.Create(actual.ContentType);
        if (actualContentTypeResult.IsFailure)
            return Error.Conflict("image.invalid.content-type", "Uploaded object has invalid content type");

        if (actualContentTypeResult.Value.Category != MediaType.IMAGE)
            return Error.Conflict("image.mismatch.content-type",
                $"Uploaded object content type {actual.ContentType} does not match policy {ALLOWED_CONTENT_TYPE}");

        if (actualContentTypeResult.Value.Value != MediaData.ContentType.Value)
            return Error.Conflict("image.mismatch.content-type",
                $"Uploaded object content type {actual.ContentType} does not match declared {MediaData.ContentType.Value}");

        return UnitResult.Success<Error>();
    }
}
