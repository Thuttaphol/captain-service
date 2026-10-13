namespace Captain.Identity;

using Captain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

public sealed class AppUserManager(
  IUserStore<AppUser> store,
  IOptions<IdentityOptions> optionsAccessor,
  IPasswordHasher<AppUser> passwordHasher,
  IEnumerable<IUserValidator<AppUser>> userValidators,
  IEnumerable<IPasswordValidator<AppUser>> passwordValidators,
  ILookupNormalizer keyNormalizer,
  IdentityErrorDescriber errors,
  IServiceProvider services,
  ILogger<UserManager<AppUser>> logger
)
  : UserManager<AppUser>(
    store,
    optionsAccessor,
    passwordHasher,
    userValidators,
    passwordValidators,
    keyNormalizer,
    errors,
    services,
    logger
  )
{
  public override Task<IdentityResult> CreateAsync(AppUser user, string password)
  {
    if (user.Categories.Count == 0)
    {
      user.Categories =
      [
        new Category { Name = "เงินเดือน", TransactionType = TransactionType.Income },
        new Category { Name = "รายได้อื่นๆ", TransactionType = TransactionType.Income },
        new Category { Name = "อาหาร", TransactionType = TransactionType.Expense },
        new Category { Name = "ค่าเดินทาง", TransactionType = TransactionType.Expense },
        new Category
        {
          Name = "ค่าใช้จ่ายอื่นๆ",
          TransactionType = TransactionType.Expense,
        },
      ];
    }

    return base.CreateAsync(user, password);
  }
}
