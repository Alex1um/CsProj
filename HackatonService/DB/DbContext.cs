using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Npgsql;

public class MyDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<MyTable> MyTables { get; set; }

    public MyDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgresConnection"));
    }
}