using RecipeShareLibrary.Model.Rights.Implementation;
using Microsoft.EntityFrameworkCore;

namespace RecipeShareLibrary.ModelBuilders.Rights;

public static class UserTokenModelBuilder
{
    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.ToTable("rgh_user_token");

            entity.HasKey(e => e.Id).HasName("rgh_user_token_pk");
            entity.HasIndex(e => e.Guid).HasDatabaseName("rgh_user_token_code_uniq_k").IsUnique();

            entity.HasIndex(e => e.UserId).HasDatabaseName("fk_user_token_user_idx");
            entity.HasIndex(e => new { e.IsActive, e.CreatedOn }).HasDatabaseName("rgh_user_inx_is_active_created");

            entity.Property(m => m.Id).HasColumnName("utoId").ValueGeneratedOnAdd();
            entity.Property(m => m.UserId).HasColumnName("utoUserId");
            entity.Property(m => m.Guid).HasColumnName("utoGuid")
                .HasConversion(x => x.ToByteArray(), x =>new Guid(x));
            entity.Property(m => m.ExpirationDate).HasColumnName("utoExpirationDate");
            entity.Property(m => m.IpAddress).HasColumnName("utoIpAddress").HasMaxLength(65535);
            entity.Property(m => m.UserAgent).HasColumnName("utoUserAgent").HasMaxLength(65535);
            entity.Property(m => m.Token).HasColumnName("utoToken").HasMaxLength(65535);
            entity.Property(m => m.IsActive).HasColumnName("utoIsActive").HasDefaultValue(true).IsRequired();
            entity.Property(m => m.CreatedOn).HasColumnName("utoCreatedOn").HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAdd();
            entity.Property(m => m.UpdatedOn).HasColumnName("utoUpdatedOn").HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAdd();

        });
    }
}