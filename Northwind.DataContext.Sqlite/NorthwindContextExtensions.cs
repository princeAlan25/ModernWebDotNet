using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Northwind.DataContext.Sqlite;

public static class NorthwindContextExtensions
{
    public static IServiceCollection AddNorthwindContext(
        this IServiceCollection services,
        string relativePath = "..",
        string databaseFilename = "Northwind.db"
        )
    {
        string databasePath = Combine(relativePath, databaseFilename);
        string fullDatabasePath = Combine(databasePath);

        if (!File.Exists(fullDatabasePath)) throw new FileNotFoundException($"Database file path {fullDatabasePath} does not exist", fileName: fullDatabasePath);
        
        NorthwindContextLogger.WriteLine($"Database Path: {fullDatabasePath}");
        services.AddDbContext<NorthwindContext>(options =>
        {
            options.UseSqlite($"Data Source={fullDatabasePath}");

            options.LogTo(
                NorthwindContextLogger.WriteLine,
                [Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting]
                );
        },
        contextLifetime: ServiceLifetime.Transient,
        optionsLifetime: ServiceLifetime.Transient
        );

        return services;
    }
}
