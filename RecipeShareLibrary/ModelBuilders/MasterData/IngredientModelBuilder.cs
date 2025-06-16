using RecipeShareLibrary.Model.MasterData.Implementation;
using Microsoft.EntityFrameworkCore;
using RecipeShareLibrary.Helper;

namespace RecipeShareLibrary.ModelBuilders.MasterData;

public static class IngredientModelBuilder
{
    private const string Prefix = "ing";

    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.ToTable("mtn_ingredient");

            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.Guid).HasDatabaseName("rcpGuid").IsUnique();

            entity.Property(m => m.Id).HasColumnNameWithPrefix(Prefix).ValueGeneratedOnAdd();
            entity.Property(m => m.Guid).HasColumnNameWithPrefix(Prefix)
                .HasConversion(x => x.ToByteArray(), x =>new Guid(x)).IsRequired();
            entity.Property(m => m.Name).HasColumnNameWithPrefix(Prefix).HasMaxLength(255).IsUnicode(false).IsRequired();

            entity.AddAuditFields(Prefix);
        });
    }
}