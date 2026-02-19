using Dapper;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Enums;
using MySqlConnector;

namespace MyRecipeBook.Infrastructure.Migrations;

public static class DatabaseMigration
{
    public static void Migrate(DatabaseType databaseType, string connectionString, IServiceProvider serviceProvider)
    {
        if(databaseType == DatabaseType.MySql)
            EnsureMySQLDatabseCreated(connectionString);
        else
            EnsureSQLServerDatabseCreated(connectionString);
        MigrationDatabase(serviceProvider);
    }

    private static void EnsureMySQLDatabseCreated(string connectionString)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);

        var databaseName = connectionStringBuilder.Database;

        const string database = "Database";
        connectionStringBuilder.Remove(database);

        using var connection = new MySqlConnection(connectionStringBuilder.ConnectionString);

        connection.Execute($"CREATE DATABASE IF NOT EXISTS `{databaseName}`;");
    }

    private static void EnsureSQLServerDatabseCreated(string connectionString)
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

        var databaseName = connectionStringBuilder.InitialCatalog;

        connectionStringBuilder.Remove("Database");

        using var connection = new SqlConnection(connectionStringBuilder.ConnectionString);

        connection.Execute(
            $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{databaseName}')"
            + " CREATE DATABASE [{databaseName}];");
    }

    private static void MigrationDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        
        runner.ListMigrations();

        runner.MigrateUp();

    }

    [Obsolete("This is the original way the course was doing")]
    private static void EnsureMySQLDatabseCreated_(string connectionString)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);

        var databaseName = connectionStringBuilder.Database;

        connectionStringBuilder.Remove("Database");

        using var connection = new MySqlConnection(connectionStringBuilder.ConnectionString);

        var parameters = new DynamicParameters();
        parameters.Add("@name", databaseName);

        var records = connection.Query(
            $"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @name;",
            parameters);

        if (records.Any())
            connection.Execute($"CREATE DATABASE `{databaseName}`;");

    }


    [Obsolete("This is the original way the course was doing")]
    private static void EnsureSQLServerDatabseCreated_(string connectionString)
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

        var databaseName = connectionStringBuilder.InitialCatalog;

        connectionStringBuilder.Remove("Database");

        using var connection = new SqlConnection(connectionStringBuilder.ConnectionString);

        var parameters = new DynamicParameters();
        parameters.Add("@name", databaseName);

        var records = connection.Query(
            $"SELECT name FROM sys.databases WHERE name = @name;",
            parameters);


        if (records.Any())
            connection.Execute($"CREATE DATABASE `{databaseName}`;");

    }
}
