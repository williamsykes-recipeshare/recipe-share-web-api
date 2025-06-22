using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using RecipeShareLibrary.Manager.Recipes.Implementation;
using RecipeShareLibrary.Model.Recipes;
using RecipeShareLibrary.Validator.Recipes.Implementation;
using RecipeShareBenchmark.Helpers;

namespace RecipeShareBenchmark.Recipes;

[MemoryDiagnoser]
public class RecipeManagerBenchmarks
{
    private RecipeManager? _recipeManager;

    private readonly RecipeFilters _filters = new RecipeFilters
    {
        Name = "chicken",
        IngredientIds = new List<long> { 1 },
        DietaryTagIds = new List<long> { 2 }
    };

    [GlobalSetup]
    public void Setup()
    {
        _recipeManager = new RecipeManager(BenchmarkWithMySql.DbFactory(), new RecipeValidator());
    }

    [Benchmark]
    public async Task<List<IRecipe>> GetFilteredListAsync()
    {
        // Await and materialize result to a list so BenchmarkDotNet can measure properly
        return (await _recipeManager!.GetFilteredListAsync(_filters)).ToList();
    }
}

class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<RecipeManagerBenchmarks>();
    }
}
