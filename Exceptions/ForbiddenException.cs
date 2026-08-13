namespace Captain.Exceptions;

internal sealed class ForbiddenException : AppException
{
  internal ForbiddenException(string message = "No access")
    : base(message, StatusCodes.Status403Forbidden, "Forbidden") { }
}
