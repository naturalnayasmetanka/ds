using DS.Domain.Models.Departments;
using DS.Domain.Models.DepartmentsPositions;
using DS.Domain.Models.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DS.Infrastructure.Configurations;

public class DepartmentPositionConfiguration : IEntityTypeConfiguration<DepartmentPosition>
{
    public void Configure(EntityTypeBuilder<DepartmentPosition> builder)
    {
        builder.ToTable("department_positions");

        builder.HasKey(dl => new { dl.DepartmentId, dl.PositionId }).HasName("pk_department_positions");

        builder.Property(dl => dl.PositionId)
            .IsRequired()
            .HasColumnName("position_id");

        builder.Property(dl => dl.DepartmentId)
            .IsRequired()
            .HasColumnName("department_id");

        builder.HasOne<Department>()
               .WithMany(d => d.DepartmentsPositions)
               .HasForeignKey(dp => dp.DepartmentId)
               .HasConstraintName("fk_department_positions_department_id")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Position>()
               .WithMany(p => p.DepartmentsPositions)
               .HasForeignKey(dp => dp.PositionId)
               .HasConstraintName("fk_department_positions_position_id")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
