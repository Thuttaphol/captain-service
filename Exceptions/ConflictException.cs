namespace Captain.Exceptions;

internal sealed class ConflictException : AppException
{
  internal ConflictException(string message, int statusCode)
    : base(message, StatusCodes.Status409Conflict, "Conflict") { }
}
