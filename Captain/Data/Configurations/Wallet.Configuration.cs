using Captain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Captain.Data.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
  public void Configure(EntityTypeBuilder<Wallet> builder)
  {
    builder.ToTable("wallet", "money");

    builder.HasKey(w => new { w.Id, w.AppUserId });
    builder.Property(w => w.Id).ValueGeneratedOnAdd();

    builder.Property(w => w.Name).HasMaxLength(50);

    builder
      .HasOne(w => w.AppUser)
      .WithMany(appUser => appUser.Wallets)
      .HasForeignKey(w => w.AppUserId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
