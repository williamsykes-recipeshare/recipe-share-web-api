using RecipeShareLibrary.Model.Rights.Implementation;
using Microsoft.EntityFrameworkCore;
using RecipeShareLibrary.Helper;

namespace RecipeShareLibrary.ModelBuilders.Rights;

public static class UserRightModelBuilder
{
    private const string Prefix = "urg";

    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRight>(entity => {
            entity.ToTable("rgh_user_right");

            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => new { e.RightId, e.UserId }).HasDatabaseName("rgh_user_right_user_right_unq_k").IsUnique();

            entity.Property(m => m.Id).HasColumnNameWithPrefix(Prefix).ValueGeneratedOnAdd();
            entity.Property(m => m.UserId).HasColumnNameWithPrefix(Prefix).IsRequired();
            entity.Property(m => m.RightId).HasColumnNameWithPrefix(Prefix).IsRequired();

            entity.AddAuditFields(Prefix);

            entity
                .HasOne<User>()
                .WithMany(m => m.UserRights)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasForeignKey(p => p.UserId)
                .HasConstraintName("fk_user_right_user");

            entity
                .HasOne(m => m.Right)
                .WithMany()
                .HasForeignKey(p => p.RightId)
                .HasConstraintName("fk_user_right_right");

            entity.Navigation(o => o.Right).IsRequired();
        });
    }
}