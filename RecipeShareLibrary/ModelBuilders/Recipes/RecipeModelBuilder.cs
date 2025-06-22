using RecipeShareLibrary.Model.Recipes.Implementation;
using Microsoft.EntityFrameworkCore;
using RecipeShareLibrary.Helper;

namespace RecipeShareLibrary.ModelBuilders.Recipes;

public static class RecipeModelBuilder
{
    private const string Prefix = "rcp";

    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.ToTable("trx_recipe");

            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.Guid).HasDatabaseName("rcpGuid").IsUnique();

            entity.Property(m => m.Id).HasColumnNameWithPrefix(Prefix).ValueGeneratedOnAdd();
            entity.Property(m => m.Guid).HasColumnNameWithPrefix(Prefix)
                .HasConversion(x => x.ToByteArray(), x =>new Guid(x)).IsRequired();
            entity.Property(m => m.Name).HasColumnNameWithPrefix(Prefix).HasMaxLength(255).IsUnicode(false).IsRequired();
            entity.Property(m => m.CookingTimeMinutes).HasColumnNameWithPrefix(Prefix).IsRequired();

            entity.AddAuditFields(Prefix);

            entity.HasMany(m => m.Steps).WithOne().HasForeignKey(p => p.RecipeId);
        });
    }
}