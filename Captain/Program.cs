using Captain.Data;
using Captain.Exceptions;
using Captain.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddLoggingConfiguration();

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

builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
  options.AddPolicy(
    name: "NextJs Origin",
    policy =>
    {
      policy
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    }
  );
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseCors("NextJs Origin");

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
