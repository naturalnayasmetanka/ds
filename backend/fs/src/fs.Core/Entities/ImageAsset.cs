using CSharpFunctionalExtensions;
using FS.Core.Enums;
using FS.Core.Exceptions;
using FS.Core.ValueObjects;

namespace FS.Core.Entities;

public class ImageAsset : MediaAsset
{
    protected ImageAsset() { }

    public ImageAsset(Guid id, MediaData mediaData, MediaStatus mediaStatus, MediaOwner mediaOwner)
        : base(id, mediaData, mediaStatus, AssetType.IMAGE, mediaOwner)
    {

    }

    public const long MAX_SIZE = 5_368_709_120;

    public const string LOCATION = "images";
    public const string RAW_PREFIX = "raw";
    public const string ALLOWED_CONTENT_TYPE = "image";

    public static readonly string[] AllowedExtensions = ["jpg", "jpeg", "png", "webp", "gif", "svg"];

    public static UnitResult<Error> Validate(MediaData mediaData)
    {
        if (!AllowedExtensions.Contains(mediaData.FileName.Extention))
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

    public static Result<ImageAsset, Error> CreateForUpload(Guid id, MediaData mediaData, MediaOwner owner)
    {
        var validationResult = Validate(mediaData);
        if (validationResult.IsFailure)
            return validationResult.Error;

        return Result.Success<ImageAsset, Error>(new ImageAsset(id, mediaData, MediaStatus.UPLOADED, owner));
    }
}
