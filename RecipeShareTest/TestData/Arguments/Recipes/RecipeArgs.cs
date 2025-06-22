using System.Collections;
using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareTest.TestData.Recipes;
using RecipeShareLibrary.Model.Recipes.Implementation;
using RecipeShareLibrary.Model.MasterData.Implementation;

namespace RecipeShareTest.TestData.Arguments.Recipes
{
    public class RecipeInvalidValidationArgs : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var recipe = RecipeData.CreateNew();
            recipe.Id = -1;
            yield return new object[] { recipe, "Invalid recipe.", typeof(NotFoundException) };

            recipe = RecipeData.CreateNew();
            recipe.Name = "";
            yield return new object[] { recipe, "Invalid name.", typeof(BadRequestException) };

            recipe = RecipeData.CreateNew();
            recipe.Name = "    ";
            yield return new object[] { recipe, "Invalid name.", typeof(BadRequestException) };

            recipe = RecipeData.CreateNew();
            recipe.Guid = new Guid();
            yield return new object[] { recipe, "Invalid guid.", typeof(BadRequestException) };

            recipe = RecipeData.CreateNew();
            recipe.CookingTimeMinutes = -10;
            yield return new object[] { recipe, "Invalid cooking time.", typeof(BadRequestException) };

            recipe = RecipeData.CreateNew();
            recipe.RecipeIngredients = new[]
            {
                new RecipeIngredient
                {
                    IngredientId = 0,
                    Quantity = 1,
                    CreatedById = 1,
                    CreatedByName = "Tester",
                    UpdatedById = 1,
                    UpdatedByName = "Tester",
                    IsActive = true,
                }
            }.ToList();
            yield return new object[] { recipe, "Invalid ingredient ID(s).", typeof(BadRequestException) };

            recipe = RecipeData.CreateNew();
            recipe.RecipeIngredients = new[]
            {
                new RecipeIngredient
                {
                    IngredientId = 1,
                    Quantity = -5,
                    CreatedById = 1,
                    CreatedByName = "Tester",
                    UpdatedById = 1,
                    UpdatedByName = "Tester",
                    IsActive = true,
                }
            }.ToList();
            yield return new object[] { recipe, "Invalid ingredient quantities.", typeof(BadRequestException) };

            recipe = RecipeData.CreateNew();
            recipe.RecipeDietaryTags = new[]
            {
                new RecipeDietaryTag
                {
                    DietaryTagId = 0,
                    CreatedById = 1,
                    CreatedByName = "Tester",
                    UpdatedById = 1,
                    UpdatedByName = "Tester",
                    IsActive = true,
                }
            }.ToList();
            yield return new object[] { recipe, "Invalid dietary tag ID(s).", typeof(BadRequestException) };

            recipe = RecipeData.CreateNew();
            recipe.Steps = new[]
            {
                new Step
                {
                    Guid = Guid.NewGuid(),
                    Name = "",
                    Index = 0,
                    CreatedById = 1,
                    CreatedByName = "Tester",
                    UpdatedById = 1,
                    UpdatedByName = "Tester",
                    IsActive = true,
                },
            }.ToList();
            yield return new object[] { recipe, "Invalid step name.", typeof(BadRequestException) };

            recipe = RecipeData.CreateNew();
            recipe.Steps = new[]
            {
                new Step
                {
                    Guid = Guid.NewGuid(),
                    Name = "   ",
                    Index = 0,
                    CreatedById = 1,
                    CreatedByName = "Tester",
                    UpdatedById = 1,
                    UpdatedByName = "Tester",
                    IsActive = true,
                },
            }.ToList();
            yield return new object[] { recipe, "Invalid step name.", typeof(BadRequestException) };

            recipe = RecipeData.CreateNew();
            recipe.Steps = new[]
            {
                new Step
                {
                    Guid = Guid.NewGuid(),
                    Name = "Step 1",
                    Index = -1,
                    CreatedById = 1,
                    CreatedByName = "Tester",
                    UpdatedById = 1,
                    UpdatedByName = "Tester",
                    IsActive = true,
                },
            }.ToList();
            yield return new object[] { recipe, "Invalid step index.", typeof(BadRequestException) };

            recipe = RecipeData.CreateNew();
            recipe.Steps = new[]
            {
                new Step
                {
                    Guid = Guid.NewGuid(),
                    Name = "Step 1",
                    Index = 0,
                    CreatedById = 1,
                    CreatedByName = "Tester",
                    UpdatedById = 1,
                    UpdatedByName = "Tester",
                    IsActive = true,
                },
                new Step
                {
                    Guid = Guid.NewGuid(),
                    Name = "Step 2",
                    Index = 0,
                    CreatedById = 1,
                    CreatedByName = "Tester",
                    UpdatedById = 1,
                    UpdatedByName = "Tester",
                    IsActive = true,
                },
            }.ToList();
            yield return new object[] { recipe, "Duplicate step indexes", typeof(BadRequestException) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class RecipeValidValidationArgs : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var recipe = RecipeData.CreateNew();
            recipe.Id = 0;
            recipe.Name = "Recipe 5";
            recipe.CookingTimeMinutes = 10;
            recipe.RecipeIngredients = new[]
            {
                new RecipeIngredient
                {
                    IngredientId = 1,
                    Quantity = 100,
                    CreatedById = 1,
                    CreatedByName = "Tester",
                    UpdatedById = 1,
                    UpdatedByName = "Tester",
                    IsActive = true,
                }
            }.ToList();
            recipe.RecipeDietaryTags = new[]
            {
                new RecipeDietaryTag
                {
                    DietaryTagId = 1,
                    CreatedById = 1,
                    CreatedByName = "Tester",
                    UpdatedById = 1,
                    UpdatedByName = "Tester",
                    IsActive = true,
                }
            }.ToList();
            recipe.Steps = new[]
            {
                new Step
                {
                    Guid = Guid.NewGuid(),
                    Name = "Step 1",
                    Index = 0,
                    CreatedById = 1,
                    CreatedByName = "Tester",
                    UpdatedById = 1,
                    UpdatedByName = "Tester",
                    IsActive = true,
                },
                new Step
                {
                    Guid = Guid.NewGuid(),
                    Name = "Step 2",
                    Index = 1,
                    CreatedById = 1,
                    CreatedByName = "Tester",
                    UpdatedById = 1,
                    UpdatedByName = "Tester",
                    IsActive = true,
                }
            }.ToList();
            yield return new object[] { recipe };

            recipe = RecipeData.CreateExisting();
            recipe.Name = "Edit";
            recipe.CookingTimeMinutes = 15;
            yield return new object[] { recipe };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class RecipeInvalidSaveArgs : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var recipe = RecipeData.CreateNew();
            recipe.Id = 500;
            yield return new object[] { 1, recipe, "Invalid recipe.", typeof(NotFoundException) };

            recipe = RecipeData.CreateNew();
            recipe.Id = 0;
            recipe.Guid = RecipeData.ExistingGuid;
            yield return new object[] { 1, recipe, "Duplicate guid.", typeof(BadRequestException) };

            using var validatorTest = new RecipeInvalidValidationArgs().GetEnumerator();
            while (validatorTest.MoveNext())
            {
                yield return new object[] { 1 }.Concat(validatorTest.Current).ToArray();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class RecipeValidSaveArgs : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            using var validatorTest = new RecipeValidValidationArgs().GetEnumerator();
            while (validatorTest.MoveNext())
            {
                yield return new object[] { 1 }.Concat(validatorTest.Current).ToArray();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}