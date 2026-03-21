using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.DataAccess;

public class MyrecipeBookDbContext(DbContextOptions<MyrecipeBookDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<Recipe> Recipes { get; set; }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyrecipeBookDbContext).Assembly);
    }
}
