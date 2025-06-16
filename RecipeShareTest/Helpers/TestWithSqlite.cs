using RecipeShareLibrary.DBContext;
using RecipeShareLibrary.DBContext.Implementation;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace RecipeShareTest.Helpers;

public class TestWithSqlite : IDisposable
{
    private bool _disposed;
    private readonly SqliteConnection _connection;

    ~TestWithSqlite() // the finalizer
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _connection.Close();
                _disposed = true;
            }
            // Release unmanaged resources.
            // Set large fields to null.
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected IRecipeShareDbContextFactory DbFactory()
    {
        var options = new DbContextOptionsBuilder<RecipeShareDbContext>()
            .UseSqlite(_connection, o => o
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
            .ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning))
            .Options;
        return new RecipeShareDbContextFactory(options);
    }

    protected TestWithSqlite()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.UnitTesting.json", optional: false);

        var configuration = builder.Build();

        _connection = new SqliteConnection(configuration.GetConnectionString("SQLite"));
        _connection.Open();

        using var dbContext = DbFactory().CreateDbContext();
        dbContext.Database.EnsureCreated();
    }
}