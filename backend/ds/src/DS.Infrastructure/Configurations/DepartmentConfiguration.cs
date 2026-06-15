using DS.Domain.Models.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DS.Infrastructure.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");

        builder.HasKey(d => d.Id)
            .HasName("pk_department");

        builder.Property(d => d.Id)
            .HasColumnName("id");

        builder.Property(d => d.Name)
            .HasConversion(d => d.Value, name => Name.Create(name).Value)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(d => d.Identifier)
            .HasConversion(d => d.Value, identifier => Identifier.Create(identifier).Value)
            .HasColumnName("identifier")
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(d => d.Path)
            .HasConversion(d => d.Value, path => Domain.Models.Departments.Path.Create(path).Value)
            .HasColumnName("path")
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(d => d.ParentId)
            .HasColumnName("parent_id");

        builder.Property(d => d.Depth)
            .HasColumnName("depth");

        builder.Property(d => d.ChildrenCount)
            .HasColumnName("children_count");

        builder.Property(d => d.IsActive)
            .HasColumnName("is_active");

        builder.Property(d => d.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(d => d.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasMany(d => d.Childrens)
                .WithOne(d => d.Parent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_parent_id");
    }
}
