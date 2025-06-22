using RecipeShareLibrary.DBContext;
using RecipeShareLibrary.DBContext.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace RecipeShareBenchmark.Helpers;

public static class BenchmarkWithMySql
{
    public static IRecipeShareDbContextFactory DbFactory()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false);

        var configuration = builder.Build();

        var connectionString = configuration.GetConnectionString("MySQL");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("MySQL connection string is missing.");

        var options = new DbContextOptionsBuilder<RecipeShareDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
            .ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning))
            .Options;

        return new RecipeShareDbContextFactory(options);
    }
}
