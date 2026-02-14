using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseType = configuration.DatabaseType();

        if (databaseType == DatabaseType.MySql) {
            {
                AddDbContext_MySql(services, configuration);
                AddFluentMigrator_MySql(services, configuration);
            }
            
        }
        else if (databaseType == DatabaseType.SqlServer) {
            AddDbContext_SqlServer(services, configuration);
            AddFluentMigrator_SqlServer(services, configuration);
        }
        else {
            throw new NotSupportedException($"Database type '{databaseType}' is not supported.");
        }
        AddRepositories(services);
    }

    private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<MyrecipeBookDbContext>(options =>
        {
            options.UseSqlServer(configuration.ConnectionString());
        });
    }

    public static void AddDbContext_MySql(IServiceCollection services, IConfiguration configuration)
    {

        var serverVersion = new MySqlServerVersion(new Version(9, 6, 0));
        services.AddDbContext<MyrecipeBookDbContext>(options =>
        {
            options.UseMySql(
                configuration.ConnectionString(),
                serverVersion);
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserWriteOlnyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
    }

    private static void AddFluentMigrator_MySql(IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddMySql5()
                .WithGlobalConnectionString(configuration.ConnectionString())
                .ScanIn(typeof(DependencyInjectionExtension).Assembly).For.Migrations());
    }
     private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(configuration.ConnectionString())
                .ScanIn(typeof(DependencyInjectionExtension).Assembly).For.Migrations());
    }
}
