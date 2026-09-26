using System.Diagnostics;
using Captain.Contracts.Errors;
using Microsoft.AspNetCore.Diagnostics;

namespace Captain.Exceptions;

internal sealed class AppExceptionHandler : IExceptionHandler
{
  private readonly ILogger<AppExceptionHandler> _logger;
  private readonly IProblemDetailsService _problemDetailsService;

  public AppExceptionHandler(
    ILogger<AppExceptionHandler> logger,
    IProblemDetailsService problemDetailsService
  )
  {
    _logger = logger;
    _problemDetailsService = problemDetailsService;
  }

  public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken
  )
  {
    var requestMethod = httpContext.Request.Method;
    var requestPath = httpContext.Request.Path;

    // In case client cancel request
    if (
      exception is OperationCanceledException
      && httpContext.RequestAborted.IsCancellationRequested
    )
    {
      _logger.LogInformation($"Request {requestMethod} {requestPath} was aborted by client.");

      if (!httpContext.Response.HasStarted)
      {
        httpContext.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;
      }

      return true;
    }

    var appException = exception as AppException;

    var title = appException?.Title ?? "An unexpected error occurred";
    var statusCode = appException?.StatusCode ?? StatusCodes.Status500InternalServerError;
    var detail =
      appException?.Message ?? "An unexpected error occurred while processing your request.";
    var traceId = Activity.Current?.TraceId.ToString() ?? httpContext.TraceIdentifier;

    //Log error
    if (statusCode >= StatusCodes.Status500InternalServerError)
    {
      _logger.LogError(
        exception,
        "Unhandled exception on {requestMethod} {requestPath}, traceId: {traceId}",
        requestMethod,
        requestPath,
        traceId
      );
    }
    else
    {
      _logger.LogWarning(
        exception,
        "Handled {exceptionType} on {requestMethod} {requestPath}: {message}, traceId: {traceId}",
        exception.GetType().Name,
        requestMethod,
        requestPath,
        exception.Message,
        traceId
      );
    }

    httpContext.Response.StatusCode = statusCode;

    var response = new ApiErrorResponse
    {
      Title = title,
      Status = statusCode,
      Detail = detail,
      TraceId = traceId,
    };

    await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

    return true;
  }
}
