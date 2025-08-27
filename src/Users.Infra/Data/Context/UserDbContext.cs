using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Users.Domain.Entities;

namespace Users.Infra.Data.Context;

public class UserDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public UserDbContext(IConfiguration configuration) : base()
    {
       _configuration = configuration;
    }

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DbConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
    }
}
