using Captain.Authorization;
using Captain.Services;
using Microsoft.AspNetCore.Authorization;
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
      services.AddSingleton<
        IAuthorizationMiddlewareResultHandler,
        ApiAuthorizationMiddlewareResultHandler
      >();

      return services;
    }
  }
}
