namespace Captain.Exceptions;

internal abstract class AppException : Exception
{
  internal AppException(string message, int statusCode, string title)
    : base(message: message)
  {
    StatusCode = statusCode;
    Title = title;
  }

  internal int StatusCode { get; }
  internal string Title { get; }
}
