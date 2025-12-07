using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Vault> Vaults { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}