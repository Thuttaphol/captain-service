using Captain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Captain.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
  public void Configure(EntityTypeBuilder<Category> builder)
  {
    builder.ToTable("category");

    builder.HasKey(c => c.Id);

    builder.Property(c => c.Name).HasMaxLength(50);
  }
}
