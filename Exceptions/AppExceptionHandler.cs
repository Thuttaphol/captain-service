using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

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

    var statusCode = appException?.StatusCode ?? StatusCodes.Status500InternalServerError;
    var title = appException?.Title ?? "An unexpected error occurred";
    var detail =
      appException?.Message ?? "An unexpected error occurred while processing your request.";

    //Log error
    if (statusCode >= StatusCodes.Status500InternalServerError)
    {
      _logger.LogError(
        exception,
        "Unhandled exception on {requestMethod} {requestPath}",
        requestMethod,
        requestPath
      );
    }
    else
    {
      _logger.LogWarning(
        exception,
        "Handled {exceptionType} on {requestMethod} {requestPath}: {message}",
        exception.GetType().Name,
        requestMethod,
        requestPath,
        exception.Message
      );
    }

    httpContext.Response.StatusCode = statusCode;
    var problemDetails = new ProblemDetails
    {
      Status = statusCode,
      Title = title,
      Detail = detail,
    };

    var problemDetailsContext = new ProblemDetailsContext
    {
      HttpContext = httpContext,
      Exception = exception,
      ProblemDetails = problemDetails,
    };

    return await _problemDetailsService.TryWriteAsync(problemDetailsContext);
  }
}
