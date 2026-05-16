using DS.Domain.Models.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DS.Infrastructure.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("positions");

        builder.HasKey(d => d.Id)
            .HasName("pk_position");

        builder.Property(d => d.Id)
            .HasColumnName("id");

        builder.Property(d => d.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(d => d.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder.Property(d => d.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(d => d.CreateAt)
            .HasColumnName("create_at")
            .IsRequired();

        builder.Property(d => d.UpdateAt)
            .HasColumnName("update_at")
            .IsRequired();
    }
}
