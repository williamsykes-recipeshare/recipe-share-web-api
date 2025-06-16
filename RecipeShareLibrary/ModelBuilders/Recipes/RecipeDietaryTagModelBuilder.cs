using Microsoft.EntityFrameworkCore;
using RecipeShareLibrary.Helper;
using RecipeShareLibrary.Model.Recipes.Implementation;

namespace RecipeShareLibrary.ModelBuilders.Recipes;

public static class RecipeDietaryTagModelBuilder
{
    private const string Prefix = "rdt";

    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeDietaryTag>(entity =>
        {
            entity.ToTable("mtn_recipe_dietary_tag");

            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => new { e.DietaryTagId, e.RecipeId }).HasDatabaseName("mtn_recipe_dietary_tag_recipe_dietary_tag_unq_k").IsUnique();

            entity.Property(m => m.Id).HasColumnNameWithPrefix(Prefix).ValueGeneratedOnAdd();
            entity.Property(m => m.DietaryTagId).HasColumnNameWithPrefix(Prefix).IsRequired();
            entity.Property(m => m.RecipeId).HasColumnNameWithPrefix(Prefix).IsRequired();

            entity.AddAuditFields(Prefix);

            entity
                .HasOne<Recipe>()
                .WithMany(m => m.RecipeDietaryTags)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasForeignKey(p => p.RecipeId)
                .HasConstraintName("fk_mtn_recipe_dietary_tag_recipe");

            entity
                .HasOne(m => m.DietaryTag)
                .WithMany()
                .HasForeignKey(p => p.DietaryTagId)
                .HasConstraintName("fk_mtn_recipe_dietary_tag_dietary_tag");

            entity.Navigation(o => o.DietaryTag).IsRequired();
        });
    }
}