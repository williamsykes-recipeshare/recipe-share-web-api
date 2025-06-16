using Microsoft.EntityFrameworkCore;
using RecipeShareLibrary.Helper;
using RecipeShareLibrary.Model.Recipes.Implementation;

namespace RecipeShareLibrary.ModelBuilders.Recipes;

public static class RecipeIngredientModelBuilder
{
    private const string Prefix = "rpi";

    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.ToTable("mtn_recipe_ingredient");

            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => new { e.IngredientId, e.RecipeId }).HasDatabaseName("mtn_recipe_ingredient_recipe_ingredient_unq_k").IsUnique();

            entity.Property(m => m.Id).HasColumnNameWithPrefix(Prefix).ValueGeneratedOnAdd();
            entity.Property(m => m.IngredientId).HasColumnNameWithPrefix(Prefix).IsRequired();
            entity.Property(m => m.RecipeId).HasColumnNameWithPrefix(Prefix).IsRequired();
            entity.Property(m => m.Quantity).HasColumnNameWithPrefix(Prefix).IsRequired();

            entity.AddAuditFields(Prefix);

            entity
                .HasOne<Recipe>()
                .WithMany(m => m.RecipeIngredients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasForeignKey(p => p.RecipeId)
                .HasConstraintName("fk_mtn_recipe_ingredient_recipe");

            entity
                .HasOne(m => m.Ingredient)
                .WithMany()
                .HasForeignKey(p => p.IngredientId)
                .HasConstraintName("fk_mtn_recipe_ingredient_ingredient");

            entity.Navigation(o => o.Ingredient).IsRequired();
        });
    }
}