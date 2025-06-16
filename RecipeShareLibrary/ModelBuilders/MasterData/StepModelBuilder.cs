using RecipeShareLibrary.Model.MasterData.Implementation;
using Microsoft.EntityFrameworkCore;
using RecipeShareLibrary.Helper;

namespace RecipeShareLibrary.ModelBuilders.MasterData;

public static class StepModelBuilder
{
    private const string Prefix = "stp";

    public static void Build(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Step>(entity =>
        {
            entity.ToTable("mtn_step");

            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.Guid).HasDatabaseName("stpGuid").IsUnique();

            entity.Property(m => m.Id).HasColumnNameWithPrefix(Prefix).ValueGeneratedOnAdd();
            entity.Property(m => m.Guid).HasColumnNameWithPrefix(Prefix)
                .HasConversion(x => x.ToByteArray(), x =>new Guid(x)).IsRequired();
            entity.Property(m => m.RecipeId).HasColumnNameWithPrefix(Prefix).IsRequired();
            entity.Property(m => m.Index).HasColumnNameWithPrefix(Prefix).IsRequired();
            entity.Property(m => m.Name).HasColumnNameWithPrefix(Prefix).HasMaxLength(255).IsUnicode(false).IsRequired();

            entity.AddAuditFields(Prefix);

            entity.HasOne(m => m.Recipe).WithMany(m => m.Steps).HasForeignKey(p => p.RecipeId);
        });
    }
}