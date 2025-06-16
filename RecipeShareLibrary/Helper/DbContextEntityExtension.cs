using RecipeShareLibrary.Model;
using RecipeShareLibrary.Model.CustomExceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RecipeShareLibrary.Helper;

public static class DbContextEntityExtension
{
    public static EntityTypeBuilder<TEntity> AddAuditFields<TEntity>(this EntityTypeBuilder<TEntity> entity, string prefix) where TEntity : BaseModel
    {
        entity.Property(m => m.IsActive).HasColumnNameWithPrefix(prefix).HasDefaultValue(true).IsRequired();
        entity.Property(m => m.CreatedById).HasColumnNameWithPrefix(prefix, "CreatedBy");
        entity.Property(m => m.CreatedByName).HasColumnNameWithPrefix(prefix).HasMaxLength(255).IsUnicode(false);
        entity.Property(m => m.CreatedOn).HasColumnNameWithPrefix(prefix).HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
        entity.Property(m => m.UpdatedById).HasColumnNameWithPrefix(prefix, "UpdatedBy");
        entity.Property(m => m.UpdatedByName).HasColumnNameWithPrefix(prefix).HasMaxLength(255).IsUnicode(false);
        entity.Property(m => m.UpdatedOn).HasColumnNameWithPrefix(prefix).HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        return entity;
    }

    public static PropertyBuilder<TProperty> HasColumnNameWithPrefix<TProperty>(this PropertyBuilder<TProperty> property, string prefix)
    {
        if (prefix == string.Empty)
            throw new EntityBuilderException("Prefix cannot be empty.");

        if (prefix.Substring(0, 1).ToUpper() == prefix.Substring(0, 1))
            throw new EntityBuilderException("Prefix cannot start with an uppercase letter.");

        property.HasColumnName($"{prefix}{property.Metadata.Name}");
        return property;
    }

    public static PropertyBuilder<TProperty> HasColumnNameWithPrefix<TProperty>(this PropertyBuilder<TProperty> property, string prefix, string columnName)
    {
        if (prefix == string.Empty)
            throw new EntityBuilderException("Prefix cannot be empty.");

        if (prefix.Substring(0, 1).ToUpper() == prefix.Substring(0, 1))
            throw new EntityBuilderException("Prefix cannot start with an uppercase letter.");

        if (columnName == string.Empty)
            throw new EntityBuilderException("Column name cannot be empty.");

        if (columnName.Substring(0, 1).ToLower() == columnName.Substring(0, 1))
            throw new EntityBuilderException("Column name cannot start with a lowercase letter.");

        property.HasColumnName($"{prefix}{columnName}");
        return property;
    }
}
