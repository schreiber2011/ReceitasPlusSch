using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.DataAccess;

public class MyrecipeBookDbContext : DbContext
{
        public MyrecipeBookDbContext(DbContextOptions<MyrecipeBookDbContext> options) : base(options)
        {
        }

    public DbSet<User> Users { get; set; }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyrecipeBookDbContext).Assembly);
    }
}
