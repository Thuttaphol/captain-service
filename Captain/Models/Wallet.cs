namespace Captain.Models;

public class Wallet
{
  public int Id { get; set; }
  public required string Name { get; set; }
  public required decimal Balance { get; set; }

  public ICollection<Transaction> Transactions { get; set; } = [];
  public string AppUserId { get; set; } = string.Empty;
  public AppUser AppUser { get; set; } = null!;
}
