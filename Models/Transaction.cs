namespace Captain.Models;

public class Transaction
{
  public int Id { get; set; }
  public required string Title { get; set; }
  public string Description { get; set; } = string.Empty;
  public TransactionType TransactionType { get; set; }
  public decimal Amount { get; set; }
  public DateTime UpdatedDate { get; set; }
  public int CategoryId { get; set; } //FK

  public Category Category { get; set; } = null!;
}
