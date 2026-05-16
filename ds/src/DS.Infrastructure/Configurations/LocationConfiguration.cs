using DS.Domain.Models.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DS.Infrastructure.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");

        builder.HasKey(d => d.Id)
            .HasName("pk_location");

        builder.Property(d => d.Id)
            .HasColumnName("id");

        builder.Property(d => d.Name)
           .HasConversion(d => d.Value, name => new Name(name))
           .HasColumnName("name")
           .IsRequired()
           .HasMaxLength(250);

        builder.OwnsOne(d => d.Address, dd =>
        {
            dd.ToJson("address");

            dd.Property(d => d.Country)
                .HasJsonPropertyName("country")
                .IsRequired()
                .HasMaxLength(100);

            dd.Property(d => d.Region)
                .HasJsonPropertyName("region")
               .IsRequired()
               .HasMaxLength(100);

            dd.Property(d => d.SettlementName)
                .HasJsonPropertyName("settlementName")
               .IsRequired()
               .HasMaxLength(100);

            dd.Property(d => d.SettlementType)
                .HasJsonPropertyName("settlementType")
               .IsRequired()
               .HasMaxLength(100);

            dd.Property(d => d.Street)
                .HasJsonPropertyName("street")
               .IsRequired()
               .HasMaxLength(100);

            dd.Property(d => d.BuildingNumber)
                .HasJsonPropertyName("buildingNumber")
               .IsRequired()
               .HasMaxLength(100);

            dd.Property(d => d.BuildingBlock)
                .HasJsonPropertyName("buildBlock")
               .HasMaxLength(100);

            dd.Property(d => d.Entrance)
                .HasJsonPropertyName("entrance")
               .IsRequired()
               .HasMaxLength(100);

            dd.Property(d => d.Floor)
                .HasJsonPropertyName("floor")
               .IsRequired()
               .HasMaxLength(100);

            dd.Property(d => d.PremiseNumber)
                .HasJsonPropertyName("premiseNumber")
               .IsRequired()
               .HasMaxLength(100);

            dd.Property(d => d.PremiseType)
                .HasJsonPropertyName("premiseType")
               .IsRequired()
               .HasMaxLength(100);

            dd.Property(d => d.PostCode)
                .HasJsonPropertyName("postCode")
               .IsRequired()
               .HasMaxLength(100);

            dd.Property(d => d.FullAddress)
                .HasJsonPropertyName("fullAddress")
               .IsRequired()
               .HasMaxLength(500);

            dd.Property(d => d.Comment)
                .HasJsonPropertyName("comment")
               .HasMaxLength(500);
        });
           
        builder.Property(d => d.Timezone)
           .HasConversion(d => d.Value, timezone => Timezone.Create(timezone).Value)
           .HasColumnName("timezone")
           .IsRequired()
           .HasMaxLength(250);

        builder.Property(d => d.IsActive)
           .HasColumnName("is_active");

        builder.Property(d => d.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(d => d.UpdatedAt)
            .HasColumnName("updated_at");
    }
}
