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

        builder.HasDiscriminator<string>("asset_type")
            .HasValue<ImageAsset>("image");

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.MediaStatus).HasConversion<string>();
        builder.Property(x => x.AssetType).HasConversion<string>();
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
                cb.Property(x => x.Category).HasConversion<string>().HasColumnName("category");
                cb.Property(x => x.Value).HasColumnName("value");
            });

            mb.OwnsOne(md => md.FileName, fb =>
            {
                fb.Property(x => x.Extention).HasColumnName("extension");
                fb.Property(x => x.Name).HasColumnName("value");
            });

            mb.Property(md => md.Size).HasColumnName("size");
            mb.Property(md => md.ExpectedChunksCount).HasColumnName("expected_chunks_count");
        });
    }
}
