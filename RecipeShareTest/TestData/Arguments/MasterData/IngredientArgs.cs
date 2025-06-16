using System.Collections;
using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareTest.TestData.MasterData;

namespace RecipeShareTest.TestData.Arguments.MasterData;


public class IngredientInvalidValidationArgs : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var ingredient = IngredientData.CreateNew();
        ingredient.Id = -1;
        yield return new object[] { ingredient, "Invalid ingredient.", typeof(NotFoundException) };

        ingredient = IngredientData.CreateNew();
        ingredient.Name = "";
        yield return new object[] { ingredient, "Invalid name.", typeof(BadRequestException) };

        ingredient = IngredientData.CreateNew();
        ingredient.Name = "    ";
        yield return new object[] { ingredient, "Invalid name.", typeof(BadRequestException) };

        ingredient = IngredientData.CreateNew();
        ingredient.Guid = new Guid();
        yield return new object[] { ingredient, "Invalid guid.", typeof(BadRequestException) };

    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class IngredientValidValidationArgs : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var ingredient = IngredientData.CreateNew();
        ingredient.Id = 0;
        ingredient.Name = "Ingredient 5";
        yield return new object[] { ingredient };

        ingredient = IngredientData.CreateExisting();
        ingredient.Name = "Edit";
        yield return new object[] { ingredient };

    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class IngredientInvalidSaveArgs : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var ingredient = IngredientData.CreateNew();
        ingredient.Id = 500;
        yield return new object[] { 1, ingredient, "Invalid ingredient.", typeof(NotFoundException)  };

        ingredient = IngredientData.CreateNew();
        ingredient.Id = 0;
        ingredient.Guid = IngredientData.ExistingGuid;
        yield return new object[] { 1, ingredient, "Duplicate guid.", typeof(BadRequestException)  };

        using var validatorTest = new IngredientInvalidValidationArgs().GetEnumerator();

        while (validatorTest.MoveNext())
        {
            yield return new object[] { 1 }.Concat(validatorTest.Current).ToArray();
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class IngredientValidSaveArgs : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        using var validatorTest = new IngredientValidValidationArgs().GetEnumerator();

        while (validatorTest.MoveNext())
        {
            yield return new object[] { 1 }.Concat(validatorTest.Current).ToArray();
        }

    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}