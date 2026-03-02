using Microsoft.Extensions.Configuration;
using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static bool IsUnitTestEnvironment(this IConfiguration configuration)
    {
        bool inMemoryTestValue = configuration.GetChildren()
            .Any(c => c.Key == "InMemoryTest" && bool.TryParse(c.Value, out var result) && result);

        return inMemoryTestValue;
    }

    public static DatabaseType DatabaseType(this IConfiguration configuration)
    {
        var databaseType = configuration.GetConnectionString("DatabaseType");
        if (string.IsNullOrWhiteSpace(databaseType))
        {
            throw new InvalidOperationException("DatabaseType connection string is missing.");
        }
        return (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType);
    }

    public static string ConnectionString(this IConfiguration configuration)
    {
        var databaseType = configuration.DatabaseType();

        if (databaseType == Domain.Enums.DatabaseType.MySql)
        {
            var mySqlConnectionString = configuration.GetConnectionString("ConnectionMySQLServer");
            if (string.IsNullOrWhiteSpace(mySqlConnectionString))
            {
                throw new InvalidOperationException("MySql connection string is missing.");
            }
            return mySqlConnectionString;
        }
        else
        {
            var sqlServerConnectionString = configuration.GetConnectionString("ConnectionSQLServer");
            if (string.IsNullOrWhiteSpace(sqlServerConnectionString))
            {
                throw new InvalidOperationException("SqlServer connection string is missing.");
            }
            return sqlServerConnectionString;
        }
    }
}
