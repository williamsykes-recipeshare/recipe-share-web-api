using RecipeShareLibrary.Model.Rights.Implementation;
using Microsoft.EntityFrameworkCore;
using RecipeShareLibrary.Helper;

namespace RecipeShareLibrary.ModelBuilders.Rights;

public static class RightModelBuilder
{
    private const string Prefix = "rgh";

    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Right>(entity => {
            entity.ToTable("rgh_right");

            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.Code).HasDatabaseName("rghCode").IsUnique();

            entity.Property(m => m.Id).HasColumnNameWithPrefix(Prefix);
            entity.Property(m => m.ParentId).HasColumnNameWithPrefix(Prefix);
            entity.Property(m => m.Code).HasColumnNameWithPrefix(Prefix);
            entity.Property(m => m.Name).HasColumnNameWithPrefix(Prefix);
            entity.Property(m => m.Url).HasColumnNameWithPrefix(Prefix).HasMaxLength(65535);
            entity.Property(m => m.IsMenu).HasColumnNameWithPrefix(Prefix);
            entity.Property(m => m.Type).HasColumnNameWithPrefix(Prefix);

            entity.AddAuditFields(Prefix);

            entity
                .HasOne<Right>()
                .WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasForeignKey(p => p.ParentId)
                .HasConstraintName("fk_right_parent");
        });
    }
}