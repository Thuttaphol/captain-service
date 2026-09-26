using Captain.Contracts.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Captain.Authorization;

public sealed class ApiAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
  private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

  public async Task HandleAsync(
    RequestDelegate requestDelegate,
    HttpContext httpContext,
    AuthorizationPolicy authorizationPolicy,
    PolicyAuthorizationResult policyAuthorizationResult
  )
  {
    if (policyAuthorizationResult.Challenged)
    {
      await _defaultHandler.HandleAsync(
        requestDelegate,
        httpContext,
        authorizationPolicy,
        policyAuthorizationResult
      );

      if (!httpContext.Response.HasStarted)
      {
        await httpContext.Response.WriteAsJsonAsync(
          new ApiErrorResponse
          {
            Title = "Unauthorized",
            Status = StatusCodes.Status401Unauthorized,
            Detail = "Authentication is required to access this resource.",
          }
        );
      }

      return;
    }

    if (policyAuthorizationResult.Forbidden)
    {
      await _defaultHandler.HandleAsync(
        requestDelegate,
        httpContext,
        authorizationPolicy,
        policyAuthorizationResult
      );

      if (!httpContext.Response.HasStarted)
      {
        await httpContext.Response.WriteAsJsonAsync(
          new ApiErrorResponse
          {
            Title = "Forbidden",
            Status = StatusCodes.Status403Forbidden,
            Detail = "You do not have permission to access this resource.",
          }
        );
      }

      return;
    }
    await _defaultHandler.HandleAsync(
      requestDelegate,
      httpContext,
      authorizationPolicy,
      policyAuthorizationResult
    );
  }
}
