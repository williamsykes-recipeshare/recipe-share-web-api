using RecipeShareLibrary.Model.Rights.Implementation;
using Microsoft.EntityFrameworkCore;

namespace RecipeShareLibrary.ModelBuilders.Rights;

public static class RightModelBuilder
{
    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Right>(entity => {
            entity.ToTable("rgh_right");

            entity.HasIndex(e => e.Code).HasDatabaseName("rgh_right_code_uniq_k").IsUnique();

            entity.Property(m => m.Id).HasColumnName("rghId");
            entity.Property(m => m.ParentId).HasColumnName("rghParentId");
            entity.Property(m => m.Code).HasColumnName("rghCode");
            entity.Property(m => m.Name).HasColumnName("rghName");
            entity.Property(m => m.Url).HasColumnName("rghURL").HasMaxLength(65535);
            entity.Property(m => m.IsMenu).HasColumnName("rghIsMenu");
            entity.Property(m => m.Type).HasColumnName("rghType");
            entity.Property(m => m.IsActive).HasColumnName("rghIsActive").HasDefaultValue(true).IsRequired();
            entity.Property(m => m.CreatedById).HasColumnName("rghCreatedBy");
            entity.Property(m => m.CreatedByName).HasColumnName("rghCreatedByName").HasMaxLength(255).IsUnicode(false);
            entity.Property(m => m.CreatedOn).HasColumnName("rghCreatedOn").HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            entity.Property(m => m.UpdatedById).HasColumnName("rghUpdatedBy");
            entity.Property(m => m.UpdatedByName).HasColumnName("rghUpdatedByName").HasMaxLength(255).IsUnicode(false);
            entity.Property(m => m.UpdatedOn).HasColumnName("rghUpdatedOn").HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            entity.HasKey(e => e.Id).HasName("rgh_right_pk");

            entity.HasOne<Right>().WithMany().OnDelete(DeleteBehavior.ClientSetNull)
                .HasForeignKey(p => p.ParentId).HasConstraintName("rgh_right_rgh_right_fk");
        });
    }
}