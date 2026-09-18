using Captain.Models;
using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
  public ICollection<Transaction> Transactions { get; set; } = [];
  public ICollection<Category> Categories { get; set; } = [];
}
