using RecipeShareLibrary.Model.Rights.Implementation;
using Microsoft.EntityFrameworkCore;

namespace RecipeShareLibrary.ModelBuilders.Rights;

public static class UserModelBuilder
{
    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("rgh_user");

            entity.HasKey(e => e.Id).HasName("rgh_user_pk");

            entity.HasIndex(e => e.Guid).HasDatabaseName("rgh_user_guid_uniq_k").IsUnique();

            entity.Property(m => m.Id).HasColumnName("usrId").ValueGeneratedOnAdd();
            entity.Property(m => m.Guid).HasColumnName("usrGuid")
                .HasConversion(x => x.ToByteArray(), x =>new Guid(x)).IsRequired();
            entity.Property(m => m.Name).HasColumnName("usrName").HasMaxLength(255).IsUnicode(false).IsRequired();
            entity.Property(m => m.Email).HasColumnName("usrEmail").IsRequired();
            entity.Property(m => m.LastLogin).HasColumnName("usrLastLogin");
            entity.Property(m => m.IsActive).HasColumnName("usrIsActive").HasDefaultValue(true).IsRequired();
            entity.Property(m => m.CreatedById).HasColumnName("usrCreatedBy");
            entity.Property(m => m.CreatedByName).HasColumnName("usrCreatedByName").HasMaxLength(255).IsUnicode(false);
            entity.Property(m => m.CreatedOn).HasColumnName("usrCreatedOn").HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            entity.Property(m => m.UpdatedById).HasColumnName("usrUpdatedBy");
            entity.Property(m => m.UpdatedByName).HasColumnName("usrUpdatedByName").HasMaxLength(255).IsUnicode(false);
            entity.Property(m => m.UpdatedOn).HasColumnName("usrUpdatedOn").HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

            entity.HasOne(o => o.UserPassword).WithOne().HasForeignKey<User>(e => e.Id)
                .IsRequired();
            entity.Navigation(o => o.UserPassword);
        });

        modelBuilder.Entity<UserPassword>(entity =>
        {
            entity.ToTable("rgh_user");

            entity.HasKey(e => e.Id).HasName("rgh_user_pk");

            entity.Property(m => m.Id).HasColumnName("usrId").ValueGeneratedOnAdd();
            entity.Property(m => m.Password).IsRequired().HasColumnName("usrPassword");
        });
    }
}