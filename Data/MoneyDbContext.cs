using Captain.Models;
using Microsoft.EntityFrameworkCore;

namespace Captain.Data;

public class MoneyDbContext : DbContext
{
  public MoneyDbContext(DbContextOptions<MoneyDbContext> options)
    : base(options) { }

  public DbSet<Transaction> Transactions => Set<Transaction>();
  public DbSet<Category> Categories => Set<Category>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.ApplyConfigurationsFromAssembly(typeof(MoneyDbContext).Assembly);
  }
}
