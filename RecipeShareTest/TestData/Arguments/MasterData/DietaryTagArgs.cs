using System.Collections;
using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareTest.TestData.MasterData;

namespace RecipeShareTest.TestData.Arguments.MasterData;


public class DietaryTagInvalidValidationArgs : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var dietaryTag = DietaryTagData.CreateNew();
        dietaryTag.Id = -1;
        yield return new object[] { dietaryTag, "Invalid dietary tag.", typeof(NotFoundException) };

        dietaryTag = DietaryTagData.CreateNew();
        dietaryTag.Name = "";
        yield return new object[] { dietaryTag, "Invalid name.", typeof(BadRequestException) };

        dietaryTag = DietaryTagData.CreateNew();
        dietaryTag.Name = "    ";
        yield return new object[] { dietaryTag, "Invalid name.", typeof(BadRequestException) };

        dietaryTag = DietaryTagData.CreateNew();
        dietaryTag.Guid = new Guid();
        yield return new object[] { dietaryTag, "Invalid guid.", typeof(BadRequestException) };

    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class DietaryTagValidValidationArgs : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var dietaryTag = DietaryTagData.CreateNew();
        dietaryTag.Id = 0;
        dietaryTag.Name = "DietaryTag 5";
        yield return new object[] { dietaryTag };

        dietaryTag = DietaryTagData.CreateExisting();
        dietaryTag.Name = "Edit";
        yield return new object[] { dietaryTag };

    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class DietaryTagInvalidSaveArgs : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var dietaryTag = DietaryTagData.CreateNew();
        dietaryTag.Id = 500;
        yield return new object[] { 1, dietaryTag, "Invalid dietary tag.", typeof(NotFoundException)  };

        dietaryTag = DietaryTagData.CreateNew();
        dietaryTag.Id = 0;
        dietaryTag.Guid = DietaryTagData.ExistingGuid;
        yield return new object[] { 1, dietaryTag, "Duplicate guid.", typeof(BadRequestException)  };

        using var validatorTest = new DietaryTagInvalidValidationArgs().GetEnumerator();

        while (validatorTest.MoveNext())
        {
            yield return new object[] { 1 }.Concat(validatorTest.Current).ToArray();
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class DietaryTagValidSaveArgs : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        using var validatorTest = new DietaryTagValidValidationArgs().GetEnumerator();

        while (validatorTest.MoveNext())
        {
            yield return new object[] { 1 }.Concat(validatorTest.Current).ToArray();
        }

    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}