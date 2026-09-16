namespace Captain.Models;

public class Category
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;

  public TransactionType TransacionType { get; set; }
  public ICollection<Transaction> Transactions { get; set; } = [];

  //For AppUser foreign key and navigation property
  public string AppUserId { get; set; } = string.Empty;
  public AppUser AppUser { get; set; } = null!;
}
