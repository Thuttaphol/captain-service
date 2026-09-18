using Captain.Services;
using Microsoft.AspNetCore.Identity;

namespace Captain.Extensions;

public static class ServiceExtension
{
  extension(IServiceCollection services)
  {
    public IServiceCollection AddApplicationServices()
    {
      //DI services
      services.AddScoped<ITransactionService, TransactionService>();
      services.AddScoped<ICategoryService, CategoryService>();
      services.AddSingleton<IEmailSender<AppUser>, FakeConfirmEmailSender>();

      return services;
    }
  }
}
