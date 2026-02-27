using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using MyRecipeBook.Infrastructure.Services.LoggedUser;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddLoggedUser(services);
        AddTokens(services, configuration);

        if (configuration.IsUnitTestEnvironment())
            return;

        var databaseType = configuration.DatabaseType();

        SetDbContextAndAddFluentMigration(services, configuration, databaseType);
    }

    private static void SetDbContextAndAddFluentMigration(IServiceCollection services, IConfiguration configuration, DatabaseType databaseType)
    {
        if (databaseType == DatabaseType.MySql)
        {
            AddDbContext_MySql(services, configuration);
            AddFluentMigrator_MySql(services, configuration);

        }
        else if (databaseType == DatabaseType.SqlServer)
        {
            AddDbContext_SqlServer(services, configuration);
            AddFluentMigrator_SqlServer(services, configuration);
        }
        else
        {
            throw new NotSupportedException($"Database type '{databaseType}' is not supported.");
        }
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
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
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

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(_ =>
            new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
        services.AddScoped<IAccessTokenValidator>(_ =>
            new JwtTokenValidator(signingKey!));
    }

    private static void AddLoggedUser(IServiceCollection services)
        => services.AddScoped<ILoggedUser, LoggedUser>();
}
