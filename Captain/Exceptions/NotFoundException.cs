namespace Captain.Exceptions;

internal sealed class NotFoundException : AppException
{
  internal NotFoundException(string message)
    : base(message, StatusCodes.Status404NotFound, "Resource not found") { }
}
