using FS.Core.Entities;
using FS.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace FS.Infrastructure.Postgres.Configurations;

public class MediaAssetConfiguration : IEntityTypeConfiguration<MediaAsset>
{
    public void Configure(EntityTypeBuilder<MediaAsset> builder)
    {
        builder.ToTable("media_assets");
        builder.HasKey(x => x.Id);
        builder.Ignore(x => x.AssetType);

        builder.HasDiscriminator<string>("asset_type")
            .HasValue<ImageAsset>("image");

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.MediaStatus).HasConversion<string>().HasColumnName("media_status");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        builder.Property(x => x.Key)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<StorageKey>(v, (JsonSerializerOptions?)null)!)
            .HasColumnName("raw_key")
            .HasColumnType("jsonb");

        builder.HasIndex(x => new
        {
            x.MediaStatus,
            x.CreatedAt,
        });

        builder.OwnsOne(m => m.MediaData, mb =>
        {
            mb.ToJson("media_data");

            mb.OwnsOne(md => md.ContentType, cb =>
            {
                cb.Property(x => x.Category).HasConversion<string>().HasJsonPropertyName("category");
                cb.Property(x => x.Value).HasJsonPropertyName("value");
            });

            mb.OwnsOne(md => md.FileName, fb =>
            {
                fb.Property(x => x.Extension).HasJsonPropertyName("extension");
                fb.Property(x => x.Name).HasJsonPropertyName("name");
            });

            mb.Property(md => md.Size).HasJsonPropertyName("size");
            mb.Property(md => md.ExpectedChunksCount).HasJsonPropertyName("expected_chunks_count");
        });

        builder.OwnsOne(m => m.ActualData, ab =>
        {
            ab.ToJson("actual_media_data");

            ab.Property(a => a.Size).HasJsonPropertyName("size");
            ab.Property(a => a.ContentType).HasJsonPropertyName("content_type");
            ab.Property(a => a.ETag).HasJsonPropertyName("etag");
        });

        //builder.OwnsOne(m => m.MediaOwner, ob =>
        //{
        //    ob.Property(x => x.Context).HasColumnName("owner_context");
        //    ob.Property(x => x.EntityId).HasColumnName("owner_entity_id");
        //});
    }
}
