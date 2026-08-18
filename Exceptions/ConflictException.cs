namespace Captain.Exceptions;

internal sealed class ConflictException : AppException
{
  internal ConflictException(string message)
    : base(message, StatusCodes.Status409Conflict, "Conflict") { }
}
