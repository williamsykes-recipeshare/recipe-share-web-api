using RecipeShareLibrary.Model.Rights.Implementation;
using Microsoft.EntityFrameworkCore;

namespace RecipeShareLibrary.ModelBuilders.Rights;

public static class UserRightModelBuilder
{
    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRight>(entity => {
            entity.ToTable("rgh_user_right");

            entity.Property(m => m.Id).HasColumnName("urgId").ValueGeneratedOnAdd();
            entity.Property(m => m.UserId).HasColumnName("urgUserId").IsRequired();
            entity.Property(m => m.RightId).HasColumnName("urgRightId").IsRequired();
            entity.Property(m => m.IsActive).HasColumnName("urgIsActive").HasDefaultValue(true).IsRequired();
            entity.Property(m => m.CreatedById).HasColumnName("urgCreatedBy");
            entity.Property(m => m.CreatedByName).HasColumnName("urgCreatedByName").HasMaxLength(255).IsUnicode(false);
            entity.Property(m => m.CreatedOn).HasColumnName("urgCreatedOn").HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            entity.Property(m => m.UpdatedById).HasColumnName("urgUpdatedBy");
            entity.Property(m => m.UpdatedByName).HasColumnName("urgUpdatedByName").HasMaxLength(255).IsUnicode(false);
            entity.Property(m => m.UpdatedOn).HasColumnName("urgUpdatedOn").HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            entity.HasKey(e => e.Id).HasName("rgh_user_right_pk");
            entity.HasIndex(e => new { e.RightId, e.UserId }).HasDatabaseName("rgh_user_right_user_right_unq_k").IsUnique();

            entity.HasOne<User>().WithMany(m => m.UserRights).OnDelete(DeleteBehavior.ClientSetNull)
                .HasForeignKey(p => p.UserId).HasConstraintName("rgh_user_right_rgh_user_fk");
            entity.HasOne(m => m.Right).WithMany().HasForeignKey(p => p.RightId).HasConstraintName("rgh_user_right_rgh_right_fk");

            entity.Navigation(o => o.Right).IsRequired();
        });
    }
}