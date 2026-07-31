using Captain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Captain.Data;

public class MoneyDbContext : IdentityUserContext<AppUser>
{
  public MoneyDbContext(DbContextOptions<MoneyDbContext> options)
    : base(options) { }

  public DbSet<Transaction> Transactions => Set<Transaction>();
  public DbSet<Category> Categories => Set<Category>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.HasDefaultSchema("identity");

    modelBuilder.ApplyConfigurationsFromAssembly(typeof(MoneyDbContext).Assembly);

    // Override Identity's explicitly configured table names.
    ConfigureIdentityTables(modelBuilder);
  }

  private static void ConfigureIdentityTables(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<AppUser>().ToTable("users", "identity");

    modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("user_claims", "identity");

    modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("user_logins", "identity");

    modelBuilder.Entity<IdentityUserToken<string>>().ToTable("user_tokens", "identity");
  }
}
