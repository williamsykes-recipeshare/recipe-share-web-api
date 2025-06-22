using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace RecipeShareBenchmark.Recipes;

[MemoryDiagnoser]
[SimpleJob(launchCount:1, warmupCount:1, iterationCount:500)]
public class RecipeControllerBenchmark
{
    private HttpClient? _client;
    private const string BaseUrl = "https://recipesharewebapi-a8fgg5atajb0gvdr.southafricanorth-01.azurewebsites.net"; // Change URL based on which service to test // TODO: appsettings?

    [GlobalSetup]
    public void Setup()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(BaseUrl);
    }

    [Benchmark]
    public async Task SequentialGetRecipes500Times()
    {
        var response = await _client!.GetAsync("/api/v1/recipe/GetList");
        response.EnsureSuccessStatusCode();
    }
}

class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<RecipeControllerBenchmark>();
    }
}