using Captain.Data;
using Microsoft.EntityFrameworkCore;

namespace Captain.Extensions;

public static class DatabaseExtension
{
  extension(IServiceCollection services)
  {
    public IServiceCollection AddDatabase(IConfiguration configuration)
    {
      var connectionString =
        configuration.GetConnectionString("PostgreSql")
        ?? throw new InvalidOperationException("Connection string 'PostgreSql' was not found.");

      services.AddDbContext<MoneyDbContext>(options =>
      {
        options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
      });

      return services;
    }
  }
}
