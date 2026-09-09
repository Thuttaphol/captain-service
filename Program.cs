using Captain.Data;
using Captain.Exceptions;
using Captain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
  builder.Configuration.GetConnectionString("PostgreSql")
  ?? throw new InvalidOperationException("Connection string 'PostgreSql' was not found.");

builder.Services.AddDbContext<MoneyDbContext>(options =>
{
  options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
});

// Add services to the container.
// Configure all controller JSON request and response to use enum names as string instead of number
builder
  .Services.AddControllers()
  .AddJsonOptions(options =>
  {
    options.JsonSerializerOptions.Converters.Add(
      new System.Text.Json.Serialization.JsonStringEnumConverter(allowIntegerValues: false)
    );

    options.AllowInputFormatterExceptionMessages = false;
  })
  .ConfigureApiBehaviorOptions(option =>
  {
    option.InvalidModelStateResponseFactory = context =>
    {
      var errors = context
        .ModelState.Where(entry => entry.Value?.Errors.Count > 0)
        .GroupBy(entry => NormalizedModelStateKey(entry.Key))
        .ToDictionary(
          group => group.Key,
          group =>
            group
              .SelectMany(entry =>
                entry.Value!.Errors.Select(error =>
                  CreateValidationMessage(entry.Key, error.ErrorMessage)
                )
              )
              .Distinct()
              .ToArray()
        );

      var problemDetails = new ValidationProblemDetails(errors)
      {
        Title = "Request Validation Failed",
        Status = StatusCodes.Status400BadRequest,
        Detail = "One or more request value is invalid.",
      };

      return new BadRequestObjectResult(problemDetails);
    };
  });

static string NormalizedModelStateKey(string key)
{
  if (key == "$")
  {
    return "body";
  }
  if (key.StartsWith("$"))
  {
    return key[2..];
  }

  return key;
}

static string CreateValidationMessage(string key, string frameworkMessage)
{
  if (key.StartsWith("$."))
  {
    var field = NormalizedModelStateKey(key);
    return field switch
    {
      "transactionType" => "The TransactionType field must be Expense or Income.",
      "body" => "Request body contains invalid JSON.",
      _ => $"Invalid value for '{field}'.",
    };
  }

  if (!string.IsNullOrWhiteSpace(frameworkMessage))
  {
    return frameworkMessage;
  }

  return $"Invalid value for '{NormalizedModelStateKey(key)}'";
}

builder
  .Services.AddIdentityApiEndpoints<AppUser>(options =>
  {
    options.SignIn.RequireConfirmedEmail = true;
  })
  .AddEntityFrameworkStores<MoneyDbContext>();

builder.Services.AddProblemDetails(options =>
  options.CustomizeProblemDetails = context =>
  {
    context.ProblemDetails.Type = null;
    context.ProblemDetails.Extensions.Clear();
  }
);
builder.Services.AddExceptionHandler<AppExceptionHandler>();

//DI services
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddSingleton<IEmailSender<AppUser>, FakeConfirmEmailSender>();

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
