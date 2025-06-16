using RecipeShareLibrary.Model.Rights.Implementation;
using Microsoft.EntityFrameworkCore;
using RecipeShareLibrary.Helper;

namespace RecipeShareLibrary.ModelBuilders.Rights;

public static class UserTokenModelBuilder
{
    private const string Prefix = "uto";

    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.ToTable("rgh_user_token");

            entity.HasKey(e => e.Id).HasName("rgh_user_token_pk");
            entity.HasIndex(e => e.Guid).HasDatabaseName("rgh_user_token_code_uniq_k").IsUnique();

            entity.HasIndex(e => e.UserId).HasDatabaseName("fk_user_token_user_idx");
            entity.HasIndex(e => new { e.IsActive, e.CreatedOn }).HasDatabaseName("rgh_user_inx_is_active_created");

            entity.Property(m => m.Id).HasColumnNameWithPrefix(Prefix).ValueGeneratedOnAdd();
            entity.Property(m => m.UserId).HasColumnNameWithPrefix(Prefix);
            entity.Property(m => m.Guid).HasColumnNameWithPrefix(Prefix)
                .HasConversion(x => x.ToByteArray(), x =>new Guid(x));
            entity.Property(m => m.ExpirationDate).HasColumnNameWithPrefix(Prefix);
            entity.Property(m => m.IpAddress).HasColumnNameWithPrefix(Prefix).HasMaxLength(65535);
            entity.Property(m => m.UserAgent).HasColumnNameWithPrefix(Prefix).HasMaxLength(65535);
            entity.Property(m => m.Token).HasColumnNameWithPrefix(Prefix).HasMaxLength(65535);

            entity.Property(m => m.IsActive).HasColumnNameWithPrefix(Prefix).HasDefaultValue(true).IsRequired();
            entity.Property(m => m.CreatedOn).HasColumnNameWithPrefix(Prefix).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAdd();
            entity.Property(m => m.UpdatedOn).HasColumnNameWithPrefix(Prefix).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAdd();
        });
    }
}