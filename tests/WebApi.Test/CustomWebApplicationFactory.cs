using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Infrastructure.DataAccess;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private MyRecipeBook.Domain.Entities.User? _user;
    private string _password = string.Empty;

    public string GetEmail() => _user?.Email ?? throw new InvalidOperationException("User not initialized");

    public string GetPassword() => _password ?? throw new InvalidOperationException("Password not initialized");

    public string GetName() => _user?.Name ?? throw new InvalidOperationException("User not initialized");

    public Guid GetUserIdentifier() => _user?.UserIdentifier ?? throw new InvalidOperationException("User not initialized");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<MyrecipeBookDbContext>));

                if (descriptor is not null)
                    services.Remove(descriptor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<MyrecipeBookDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                using var scope = services.BuildServiceProvider().CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<MyrecipeBookDbContext>();

                dbContext.Database.EnsureDeleted();

                StartDatabase(dbContext);
            });
    }

    private void StartDatabase(MyrecipeBookDbContext dbContext)
    {
        (_user, _password) = UserBuilder.Build();

        dbContext.Users.Add(_user);

        dbContext.SaveChanges();
    }
}
