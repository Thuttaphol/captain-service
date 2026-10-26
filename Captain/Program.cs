using Captain.Data;
using Captain.Exceptions;
using Captain.Extensions;
using Captain.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddLoggingConfiguration();

builder.Services.AddDatabase(builder.Configuration);

builder
  .Services.AddIdentityApiEndpoints<AppUser>(options =>
  {
    options.SignIn.RequireConfirmedEmail = true;
  })
  .AddEntityFrameworkStores<MoneyDbContext>()
  .AddUserManager<AppUserManager>();

builder.Services.AddErrorConfiguration();

builder.Services.AddExceptionHandler<AppExceptionHandler>();

builder.Services.AddApplicationServices();

builder.Services.AddOpenApi();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Apply any pending EF Core migrations on startup.
await using (var scope = app.Services.CreateAsyncScope())
{
  var dbContext = scope.ServiceProvider.GetRequiredService<MoneyDbContext>();

  await dbContext.Database.MigrateAsync();
}

if (!app.Environment.IsDevelopment())
{
  app.UseHttpsRedirection();
}

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/api/auth").MapIdentityApi<AppUser>();

app.MapControllers();

app.MapHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference();
}

app.Run();
