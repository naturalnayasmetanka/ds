using DS.Domain.Models.Departments;
using DS.Domain.Models.DepartmentsLocations;
using DS.Domain.Models.Locations;
using DS.Domain.Models.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DS.Infrastructure.Configurations;

public class DepartmentLocationConfiguration : IEntityTypeConfiguration<DepartmentLocation>
{
    public void Configure(EntityTypeBuilder<DepartmentLocation> builder)
    {
        builder.ToTable("department_locations");

        builder.HasKey(dl => new { dl.DepartmentId, dl.LocationId }).HasName("pk_department_locations");

        builder.Property(dl => dl.LocationId).HasColumnName("location_id");
        builder.Property(dl => dl.DepartmentId).HasColumnName("department_id");

        builder.HasOne<Department>()
              .WithMany(d => d.DepartmentsLocations)
              .HasForeignKey(dp => dp.DepartmentId)
              .HasConstraintName("fk_department_locaitons_department_id")
              .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Location>()
               .WithMany(p => p.DepartmentsLocations)
               .HasForeignKey(dp => dp.LocationId)
               .HasConstraintName("fk_department_locations_location_id")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
