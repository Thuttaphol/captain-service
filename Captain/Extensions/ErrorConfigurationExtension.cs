using System.Text.Json.Serialization;
using Captain.Contracts.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Captain.Extensions;

public static class ErrorConfigurationExtension
{
  extension(IServiceCollection services)
  {
    public IServiceCollection AddErrorConfiguration()
    {
      // Configure all controller JSON request and response to use enum names as string instead of number
      services
        .AddControllers(options =>
          options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true
        )
        .AddJsonOptions(options =>
        {
          options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict;

          options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(allowIntegerValues: false)
          );

          //turn off JSON deserialize error message
          options.AllowInputFormatterExceptionMessages = false;
        })
        .ConfigureApiBehaviorOptions(option =>
        {
          option.InvalidModelStateResponseFactory = context =>
          {
            var errors = context
              .ModelState.Where(entry => entry.Value?.Errors.Count > 0)
              .GroupBy(entry => Utils.NormalizedModelStateKey(entry.Key))
              .ToDictionary(
                group => group.Key,
                group =>
                  group
                    .SelectMany(entry =>
                      entry.Value!.Errors.Select(error =>
                        Utils.CreateValidationMessage(entry.Key, error.ErrorMessage)
                      )
                    )
                    .Distinct()
                    .ToArray()
              );

            var response = new ApiValidationErrorResponse()
            {
              Title = "Request Validation Failed",
              Status = StatusCodes.Status400BadRequest,
              Detail = "One or more request value is invalid.",
              Errors = errors,
            };

            return new BadRequestObjectResult(response);
          };
        });

      //Configure OpenAPI json for generate doc
      services.ConfigureHttpJsonOptions(options =>
      {
        options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;

        options.SerializerOptions.Converters.Add(
          new JsonStringEnumConverter(allowIntegerValues: false)
        );
      });

      services.AddProblemDetails(options =>
        options.CustomizeProblemDetails = context =>
        {
          context.ProblemDetails.Type = null;
          context.ProblemDetails.Extensions.Clear();
        }
      );

      return services;
    }
  }
}

class Utils
{
  public static string NormalizedModelStateKey(string key)
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

  public static string CreateValidationMessage(string key, string frameworkMessage)
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
}
