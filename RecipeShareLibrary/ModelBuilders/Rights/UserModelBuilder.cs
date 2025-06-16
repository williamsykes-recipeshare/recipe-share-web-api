using RecipeShareLibrary.Model.Rights.Implementation;
using Microsoft.EntityFrameworkCore;
using RecipeShareLibrary.Helper;

namespace RecipeShareLibrary.ModelBuilders.Rights;

public static class UserModelBuilder
{
    private const string Prefix = "usr";

    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("rgh_user");

            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.Guid).HasDatabaseName("usrGuid").IsUnique();
            entity.HasIndex(e => e.Email).HasDatabaseName("usrEmail").IsUnique();

            entity.Property(m => m.Id).HasColumnNameWithPrefix(Prefix).ValueGeneratedOnAdd();
            entity.Property(m => m.Guid).HasColumnNameWithPrefix(Prefix)
                .HasConversion(x => x.ToByteArray(), x =>new Guid(x)).IsRequired();
            entity.Property(m => m.Name).HasColumnNameWithPrefix(Prefix).HasMaxLength(255).IsUnicode(false).IsRequired();
            entity.Property(m => m.Email).HasColumnNameWithPrefix(Prefix).IsRequired();
            entity.Property(m => m.LastLogin).HasColumnNameWithPrefix(Prefix);
            entity.Property(m => m.IsActive).HasColumnNameWithPrefix(Prefix).HasDefaultValue(true).IsRequired();

            entity.AddAuditFields(Prefix);

            entity.HasOne(o => o.UserPassword).WithOne().HasForeignKey<User>(e => e.Id)
                .IsRequired();
            entity.Navigation(o => o.UserPassword);
        });

        modelBuilder.Entity<UserPassword>(entity =>
        {
            entity.ToTable("rgh_user");

            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(m => m.Id).HasColumnNameWithPrefix(Prefix).ValueGeneratedOnAdd();
            entity.Property(m => m.Password).IsRequired().HasColumnNameWithPrefix(Prefix);
        });
    }
}