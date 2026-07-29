using Captain.Data;
using Captain.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
  builder.Configuration.GetConnectionString("PostgreSql")
  ?? throw new InvalidOperationException("Connection string 'PostgreSql' was not found.");

builder.Services.AddDbContext<MoneyDbContext>(options =>
{
  options.UseNpgsql(connectionString);
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
  });

//DI services
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
