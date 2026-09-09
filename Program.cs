using Captain.Data;
using Captain.Exceptions;
using Captain.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddErrorConfiguration();

builder
  .Services.AddIdentityApiEndpoints<AppUser>(options =>
  {
    options.SignIn.RequireConfirmedEmail = true;
  })
  .AddEntityFrameworkStores<MoneyDbContext>();

builder.Services.AddExceptionHandler<AppExceptionHandler>();

builder.Services.AddApplicationServices();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/api/auth").MapIdentityApi<AppUser>();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference();
}

app.Run();
