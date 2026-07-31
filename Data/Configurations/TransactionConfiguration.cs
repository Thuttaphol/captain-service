using Captain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Captain.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
  public void Configure(EntityTypeBuilder<Transaction> builder)
  {
    builder.ToTable("transaction", "money");

    builder.HasKey(transaction => transaction.Id);

    builder.Property(transaction => transaction.Title).HasMaxLength(20);

    builder.Property(transaction => transaction.Description).HasMaxLength(200);

    builder.Property(transaction => transaction.Amount).HasPrecision(10, 2);

    builder
      .Property(transaction => transaction.UpdatedDate)
      .HasColumnType("timestamp with time zone");

    builder
      .HasOne(transaction => transaction.Category)
      .WithMany(category => category.Transactions)
      .HasForeignKey(transaction => transaction.CategoryId)
      .OnDelete(DeleteBehavior.Restrict);

    builder
      .HasOne(transaction => transaction.AppUser)
      .WithMany(appUser => appUser.Transactions)
      .HasForeignKey(transaction => transaction.AppUserId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
